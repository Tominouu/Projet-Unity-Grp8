//
//  Outline.cs
//  QuickOutline
//
//  Created by Chris Nolet on 3/30/18.
//  Copyright © 2018 Chris Nolet. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

[DisallowMultipleComponent]

public class Outline : MonoBehaviour {
  private static HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();

  // Référence au toggle d'accessibilité
  private static Toggle accessibilityToggle;

  // Clé pour les PlayerPrefs
  private static string prefsKey = "AccessibilityMode";

  public enum Mode {
    OutlineAll,
    OutlineVisible,
    OutlineHidden,
    OutlineAndSilhouette,
    SilhouetteOnly
  }

  public Mode OutlineMode {
    get { return outlineMode; }
    set {
      outlineMode = value;
      needsUpdate = true;
    }
  }

  public Color OutlineColor {
    get { return outlineColor; }
    set {
      outlineColor = value;
      needsUpdate = true;
    }
  }

  public float OutlineWidth {
    get { return outlineWidth; }
    set {
      outlineWidth = value;
      needsUpdate = true;
    }
  }

  [Serializable]
  private class ListVector3 {
    public List<Vector3> data;
  }

  [SerializeField]
  private Mode outlineMode;

  [SerializeField]
  private Color outlineColor = Color.white;

  [SerializeField, Range(0f, 10f)]
  private float outlineWidth = 2f;

  [Header("Accessibility")]
  [SerializeField]
  private Toggle accessibilityToggleRef;

  [Header("Optional")]

  [SerializeField, Tooltip("Precompute enabled: Per-vertex calculations are performed in the editor and serialized with the object. "
  + "Precompute disabled: Per-vertex calculations are performed at runtime in Awake(). This may cause a pause for large meshes.")]
  private bool precomputeOutline;

  [SerializeField, HideInInspector]
  private List<Mesh> bakeKeys = new List<Mesh>();

  [SerializeField, HideInInspector]
  private List<ListVector3> bakeValues = new List<ListVector3>();

  private Renderer[] renderers;
  private Material outlineMaskMaterial;
  private Material outlineFillMaterial;

  private bool needsUpdate;
  private bool isAccessibilityEnabled;

  void Awake() {
    // Initialiser le toggle d'accessibilité si ce n'est pas déjà fait
    if (accessibilityToggle == null && accessibilityToggleRef != null) {
      SetupAccessibilityToggle(accessibilityToggleRef);
    }

    // Charger l'état d'accessibilité
    isAccessibilityEnabled = PlayerPrefs.GetInt(prefsKey, 0) == 1;

    // Cache renderers
    renderers = GetComponentsInChildren<Renderer>();

    // Instantiate outline materials
    outlineMaskMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineMask"));
    outlineFillMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineFill"));

    outlineMaskMaterial.name = "OutlineMask (Instance)";
    outlineFillMaterial.name = "OutlineFill (Instance)";

    // Retrieve or generate smooth normals
    LoadSmoothNormals();

    // Apply material properties immediately
    needsUpdate = true;
  }

  void Start() {
    // Si c'est le premier objet avec ce component qui trouve le toggle
    if (accessibilityToggle == null && accessibilityToggleRef != null) {
      SetupAccessibilityToggle(accessibilityToggleRef);
    }

    // Mettre à jour l'état selon le mode d'accessibilité
    UpdateVisibilityBasedOnAccessibility();
  }

  // Configurer le toggle d'accessibilité
  private void SetupAccessibilityToggle(Toggle toggle) {
    accessibilityToggle = toggle;

    // Charger l'état précédent
    bool savedState = PlayerPrefs.GetInt(prefsKey, 0) == 1;

    // Définir l'état initial du toggle sans déclencher d'événement
    accessibilityToggle.SetIsOnWithoutNotify(savedState);

    // Configurer l'écouteur d'événement
    accessibilityToggle.onValueChanged.RemoveAllListeners();
    accessibilityToggle.onValueChanged.AddListener(OnAccessibilityToggleChanged);
  }

  // Gestionnaire d'événement pour le changement du toggle
  private static void OnAccessibilityToggleChanged(bool isOn) {
    Debug.Log("Mode accessibilité: " + (isOn ? "activé" : "désactivé"));

    // Sauvegarder l'état
    PlayerPrefs.SetInt(prefsKey, isOn ? 1 : 0);
    PlayerPrefs.Save();

    // Mettre à jour tous les outlines
    UpdateAllOutlines();
  }

  // Mettre à jour tous les outlines dans la scène
  private static void UpdateAllOutlines() {
    Outline[] outlines = FindObjectsOfType<Outline>();
    bool isEnabled = PlayerPrefs.GetInt(prefsKey, 0) == 1;

    foreach (Outline outline in outlines) {
      outline.isAccessibilityEnabled = isEnabled;
      outline.UpdateVisibilityBasedOnAccessibility();
    }
  }

  // Mettre à jour la visibilité basée sur le mode d'accessibilité
  private void UpdateVisibilityBasedOnAccessibility() {
    bool shouldBeEnabled = isAccessibilityEnabled;

    if (this.enabled != shouldBeEnabled) {
      this.enabled = shouldBeEnabled;
    }
  }

  void OnEnable() {
    foreach (var renderer in renderers) {
      // Append outline shaders
      var materials = renderer.sharedMaterials.ToList();

      materials.Add(outlineMaskMaterial);
      materials.Add(outlineFillMaterial);

      renderer.materials = materials.ToArray();
    }
  }

  void OnValidate() {
    // Si le toggle de référence a changé
    if (accessibilityToggle == null && accessibilityToggleRef != null) {
      SetupAccessibilityToggle(accessibilityToggleRef);
    }

    // Update material properties
    needsUpdate = true;

    // Clear cache when baking is disabled or corrupted
    if (!precomputeOutline && bakeKeys.Count != 0 || bakeKeys.Count != bakeValues.Count) {
      bakeKeys.Clear();
      bakeValues.Clear();
    }

    // Generate smooth normals when baking is enabled
    if (precomputeOutline && bakeKeys.Count == 0) {
      Bake();
    }
  }

  void Update() {
    // Vérifier si l'état d'accessibilité a changé
    bool currentAccessibilityState = PlayerPrefs.GetInt(prefsKey, 0) == 1;
    if (isAccessibilityEnabled != currentAccessibilityState) {
      isAccessibilityEnabled = currentAccessibilityState;
      UpdateVisibilityBasedOnAccessibility();
    }

    if (needsUpdate) {
      needsUpdate = false;
      UpdateMaterialProperties();
    }
  }

  void OnDisable() {
    foreach (var renderer in renderers) {
      // Remove outline shaders
      var materials = renderer.sharedMaterials.ToList();

      materials.Remove(outlineMaskMaterial);
      materials.Remove(outlineFillMaterial);

      renderer.materials = materials.ToArray();
    }
  }

  void OnDestroy() {
    // Destroy material instances
    Destroy(outlineMaskMaterial);
    Destroy(outlineFillMaterial);
  }

  void Bake() {
    // Generate smooth normals for each mesh
    var bakedMeshes = new HashSet<Mesh>();

    foreach (var meshFilter in GetComponentsInChildren<MeshFilter>()) {
      // Skip duplicates
      if (!bakedMeshes.Add(meshFilter.sharedMesh)) {
        continue;
      }

      // Serialize smooth normals
      var smoothNormals = SmoothNormals(meshFilter.sharedMesh);

      bakeKeys.Add(meshFilter.sharedMesh);
      bakeValues.Add(new ListVector3() { data = smoothNormals });
    }
  }

  void LoadSmoothNormals() {
    // Retrieve or generate smooth normals
    foreach (var meshFilter in GetComponentsInChildren<MeshFilter>()) {
      // Skip if smooth normals have already been adopted
      if (!registeredMeshes.Add(meshFilter.sharedMesh)) {
        continue;
      }

      // Retrieve or generate smooth normals
      var index = bakeKeys.IndexOf(meshFilter.sharedMesh);
      var smoothNormals = (index >= 0) ? bakeValues[index].data : SmoothNormals(meshFilter.sharedMesh);

      // Store smooth normals in UV3
      meshFilter.sharedMesh.SetUVs(3, smoothNormals);

      // Combine submeshes
      var renderer = meshFilter.GetComponent<Renderer>();

      if (renderer != null) {
        CombineSubmeshes(meshFilter.sharedMesh, renderer.sharedMaterials);
      }
    }

    // Clear UV3 on skinned mesh renderers
    foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>()) {
      // Skip if UV3 has already been reset
      if (!registeredMeshes.Add(skinnedMeshRenderer.sharedMesh)) {
        continue;
      }

      // Clear UV3
      skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];

      // Combine submeshes
      CombineSubmeshes(skinnedMeshRenderer.sharedMesh, skinnedMeshRenderer.sharedMaterials);
    }
  }

  List<Vector3> SmoothNormals(Mesh mesh) {
    // Group vertices by location
    var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);

    // Copy normals to a new list
    var smoothNormals = new List<Vector3>(mesh.normals);

    // Average normals for grouped vertices
    foreach (var group in groups) {
      // Skip single vertices
      if (group.Count() == 1) {
        continue;
      }

      // Calculate the average normal
      var smoothNormal = Vector3.zero;

      foreach (var pair in group) {
        smoothNormal += smoothNormals[pair.Value];
      }

      smoothNormal.Normalize();

      // Assign smooth normal to each vertex
      foreach (var pair in group) {
        smoothNormals[pair.Value] = smoothNormal;
      }
    }

    return smoothNormals;
  }

  void CombineSubmeshes(Mesh mesh, Material[] materials) {
    // Skip meshes with a single submesh
    if (mesh.subMeshCount == 1) {
      return;
    }

    // Skip if submesh count exceeds material count
    if (mesh.subMeshCount > materials.Length) {
      return;
    }

    // Append combined submesh
    mesh.subMeshCount++;
    mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
  }

  void UpdateMaterialProperties() {
    // Apply properties according to mode
    outlineFillMaterial.SetColor("_OutlineColor", outlineColor);

    switch (outlineMode) {
      case Mode.OutlineAll:
        outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
        outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
        outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
        break;

      case Mode.OutlineVisible:
        outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
        outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
        outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
        break;

      case Mode.OutlineHidden:
        outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
        outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
        outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
        break;

      case Mode.OutlineAndSilhouette:
        outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
        outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
        outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
        break;

      case Mode.SilhouetteOnly:
        outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
        outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
        outlineFillMaterial.SetFloat("_OutlineWidth", 0f);
        break;
    }
  }
}
