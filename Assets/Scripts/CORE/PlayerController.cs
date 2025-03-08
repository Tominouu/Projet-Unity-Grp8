using UnityEngine;

public class PlayerController : MonoBehaviour
{   
	//Vitesse en marchant et en courant
    [SerializeField] private float walk, run;

	//Sensibilité de la souris
	private float sensitivity = 500;
	private float speed;

    private bool isMoving = false;
	private bool isRunning = false;
	
	private CharacterController cc;

    private float X, Y;
	
	//Pour les bruits de pas
	[SerializeField] private AudioClip[] sfx_steps;
	private int num_step = 0;
	private float step_timer = 0.0f;
	private float max_step_timer = 0.5f;
	private AudioSource audio_steps;

    private void Start()
    {
        speed = walk;
        cc = GetComponent<CharacterController>();
		audio_steps = GetComponent<AudioSource>();
        cc.enabled = true;
		
		//Rend le curseur invisible
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
    }
    private void Update()
    {
		if(!HudManager.pause){
			//Camera limitation variables
			/*
			const float MIN_Y = -60.0f;
			const float MAX_Y = 70.0f;

			Y -= Input.GetAxis("Mouse Y") * (sensitivity * Time.deltaTime);

			/*
			if (Y < MIN_Y)
				Y = MIN_Y;
			else if (Y > MAX_Y)
				Y = MAX_Y;
			*/
			
			X += Input.GetAxis("Mouse X") * (sensitivity * Time.deltaTime);

			transform.localRotation = Quaternion.Euler(Y, X, 0.0f);

			float horizontal = Input.GetAxis("Horizontal");
			float vertical = Input.GetAxis("Vertical");
			Vector3 forward = transform.forward * vertical;
			Vector3 right = transform.right * horizontal;

			cc.SimpleMove((forward + right) * speed);
			
			//Si on appuie sur Shift Gauche, on court
			if (Input.GetKey(KeyCode.LeftShift))
			{
				speed = run;
				isRunning = true;
			}
			else
			{
				isRunning = false;
				speed = walk;
			}
			
			if(horizontal != 0 || vertical != 0){ //Si le joueur se déplace, on joue des bruits de pas
				if(step_timer <= 0){
					audio_steps.clip = sfx_steps[num_step];
					audio_steps.Play();
					
					step_timer = max_step_timer;
					if(isRunning){ //S'il court, on divise par 2 le temps avant d'entendre un nouveau pas
						step_timer /= 2;
					}
					num_step = (num_step+1)%sfx_steps.Length;
				} else {
					step_timer -= Time.deltaTime;
				}
			} else {
				step_timer = 0.1f;
			}
		}
    }
}