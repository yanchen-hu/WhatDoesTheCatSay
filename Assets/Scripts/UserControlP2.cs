using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserControlP2 : MonoBehaviour
{
    CharacterController playerController;

    Vector3 direction;

    public float speed = 1;
    public float jumpPower = 5;
    public float gravity = 7f;

    public float mousespeed = 5f;

    public float minmouseY = -45f;
    public float maxmouseY = 45f;

    float RotationY = 0f;
    float RotationX = 0f;

    public Transform agretctCamera;
    public GameObject ProgressionBar;
    public GameObject WakeUpBar;
    public GameObject Blind;
    public bool GetDistracted;

    public enum ATTRACTION_TYPE
    {
        NONE,
        SUN_LIGHT,
        BUTTERFLY,
    }
    private ATTRACTION_TYPE currentAttractionType;
    private float timer;
    private float coolDownTimer;
    private Transform targetPos;

    // Use this for initialization
    void Start()
    {
        playerController = this.GetComponent<CharacterController>();
        GetDistracted = false;
        timer = 0;
        coolDownTimer = GameManager.Instance.DistractCoolDownTime;
        currentAttractionType = ATTRACTION_TYPE.NONE;
    }

    // Update is called once per frame
    void Update()
    {
        if(GetDistracted)
        {
            WakeUpBar.SetActive(true);
            WakeUpBar.GetComponent<CircularProgressBar>().m_FillAmount = (1-timer / GameManager.Instance.DistractTime);
            string progress = string.Format("{0:N0}", (1 - timer / GameManager.Instance.DistractTime) * 100);
            WakeUpBar.GetComponentInChildren<Text>().text = progress + "%";
            //SUN LIGHT SPECIFIC CODE
            if(currentAttractionType == ATTRACTION_TYPE.SUN_LIGHT)
            {
                Vector3 targetPosFixed = targetPos.position;
                targetPosFixed.y = this.transform.position.y;
                this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosFixed, 2 * Time.deltaTime);
                if (Vector3.Distance(this.transform.position, targetPosFixed) <= 1)
                {
                    Blind.SetActive(true);
                }
            }
            else
            {
                Vector3 targetPosFixed = targetPos.position;
                targetPosFixed.y = this.transform.position.y;
                if (Vector3.Distance(this.transform.position, targetPosFixed) >= 3)
                {
                    this.transform.position = Vector3.MoveTowards(this.transform.position, targetPosFixed, 2 * Time.deltaTime);
                }
                agretctCamera.LookAt(targetPosFixed);


            }
           
            timer -= Time.deltaTime;
            if(timer<=0)
            {
                GetDistracted = false;
                timer = 0;
                coolDownTimer = GameManager.Instance.DistractCoolDownTime;
                print("Heading to Sun End");
                WakeUpBar.SetActive(false);
                Blind.SetActive(false);
            }
            return;
        }
        else
        {
            WakeUpBar.SetActive(false);
        }
        coolDownTimer -= Time.deltaTime;
        if (GameManager.Instance.CurrentPlayer == GameManager.PLAYER_CONTROLLER.P2 && !GameManager.Instance.GameEnd)
        {
            float _horizontal = Input.GetAxis("Horizontal");
            float _vertical = Input.GetAxis("Vertical");

            if(_horizontal!=0 || _vertical!=0)
            {
                this.GetComponentInChildren<Animator>().SetBool("IsWalk",true);
            }
            else
            {
                this.GetComponentInChildren<Animator>().SetBool("IsWalk", false);
            }
             if (playerController.isGrounded)
             {
                 direction = new Vector3(_horizontal, 0, _vertical);
                 if (Input.GetKeyDown(KeyCode.Space))
                     direction.y = jumpPower;
             }
            direction.y -= gravity * Time.deltaTime;
            playerController.Move(playerController.transform.TransformDirection(direction * Time.deltaTime * speed));

            RotationX += agretctCamera.transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mousespeed;

            RotationY -= Input.GetAxis("Mouse Y") * mousespeed;
            RotationY = Mathf.Clamp(RotationY, minmouseY, maxmouseY);

            this.transform.eulerAngles = new Vector3(0, RotationX, 0);

            agretctCamera.transform.eulerAngles = new Vector3(RotationY, RotationX, 0);

            if(coolDownTimer <= 0)
            {
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.collider.tag == "SUN" || hit.collider.tag == "BUTTERFLY")
                    {
                        timer += Time.deltaTime;
                        targetPos = hit.transform;
                        ProgressionBar.SetActive(true);
                        print("timer is counting...");
                    }
                    else
                    {
                        timer = 0;
                    }
                }
                else
                {
                    timer = 0;
                }

                if (timer >= GameManager.Instance.SteerTime)
                {
                    print("Heading to Sun");
                    ProgressionBar.SetActive(false);
                    GetDistracted = true;
                    currentAttractionType = hit.collider.tag == "SUN" ? ATTRACTION_TYPE.SUN_LIGHT : ATTRACTION_TYPE.BUTTERFLY;
                    timer = GameManager.Instance.DistractTime;
                }
                
            }
            UpdateProgressionBar();
        }
    }

    private void UpdateProgressionBar()
    {
        if(timer == 0)
        {
            ProgressionBar.SetActive(false);
        }
        ProgressionBar.GetComponent<CircularProgressBar>().m_FillAmount = timer / GameManager.Instance.SteerTime;
        string progress = string.Format("{0:N0}", timer / GameManager.Instance.SteerTime * 100);
        ProgressionBar.GetComponentInChildren<Text>().text = progress + "%";
    }
}
