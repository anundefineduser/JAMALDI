using UnityEngine;
using UnityEngine.AI;

public class SuperintendentScript : MonoBehaviour
{
    private bool seesPlayer;

    public bool activateOnDistance;

    public bool randomCoolDown;

    private Transform player;

    public BaldiScript baldiScript;

    public Transform wanderTarget;

    public AILocationSelectorScript wanderer;

    public float coolDown;

    public AudioClip aud_CallBaldi;

    private NavMeshAgent agent;

    private AudioQueueScript audioQueue;

    private AudioSource audioDevice;

    private RaycastHit hit;

    private Vector3 aim;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        audioDevice = base.gameObject.AddComponent<AudioSource>();
        wanderer = Object.FindObjectOfType<AILocationSelectorScript>();
        wanderTarget = GameObject.Find("/AIWanderPoints/AI_LocationSelector").transform;
        player = GameObject.Find("Player").transform;
    }

    private void Update()
    {
        if (seesPlayer)
        {
            CallBaldi();
        }
        if (coolDown > 0f)
        {
            coolDown -= Time.deltaTime;
        }
    }

    private void FixedUpdate()
    {
        if (coolDown <= 0f)
        {
            if (agent.velocity.magnitude <= 1f)
            {
                Wander();
            }
            aim = player.position - base.transform.position;
            if (Physics.Raycast(base.transform.position, aim, out hit, float.PositiveInfinity, 769, QueryTriggerInteraction.Ignore) && (hit.transform.tag == "Player"))
            {
                if (activateOnDistance)
                {
                    if ((base.transform.position - player.position).magnitude <= 60f)
                    {
                        seesPlayer = true;
                    }
                }
                else
                {
                    seesPlayer = true;
                }
            }
        }
        if (agent.velocity.magnitude <= 1f)
        {
            Wander();
        }
    }

    private void Wander()
    {
        wanderer.GetNewTarget();
        agent.SetDestination(wanderTarget.position);
    }

    private void CallBaldi()
    {
        Debug.Log("CallBaldi Worked");
        if (randomCoolDown)
        {
            coolDown = Random.Range(30f, 150f);
        }
        else
        {
            coolDown = 90f;
        }
        seesPlayer = false;
        audioDevice.PlayOneShot(aud_CallBaldi);
        baldiScript.Hear(player.position, 2f);
        Debug.Log("Baldi heard the Superintendent");
    }
}
