using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchOnline : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject playerPrefab;
    private GameObject playerinstance1, playerinstance2;

    public Transform[] spawnPoints;

    [SerializeField] float scorep1, scorep2;

    [SerializeField] GameObject Ball;

    public GameObject ballingame;

    [SerializeField] TextMesh textp1, textp2;

    public static MatchOnline instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogError("More than one MatchOnline instance found!");
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SpawnPlayer();
    }

    private void SpawnPlayer()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount <= spawnPoints.Length)
        {
            int index = PhotonNetwork.LocalPlayer.ActorNumber - 1;
            Transform spawnPoint = spawnPoints[index];

            Debug.LogFormat("Instantiating LocalPlayer at {0} from {1}", spawnPoint.position, SceneManager.GetActiveScene().name);

            PhotonNetwork.Instantiate(playerPrefab.name, spawnPoint.position, spawnPoint.rotation, 0);
        }
        else
        {
            Debug.LogError("Not enough spawn points for the number of players. Player not spawned.");
        }
    }

    public override void OnPlayerEnteredRoom(Player other)
    {
        Debug.Log("OnPlayerEnteredRoom() " + other.NickName);

        if (PhotonNetwork.IsMasterClient)
        {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);

          
        }
    }

    public override void OnPlayerLeftRoom(Player other)
    {
        Debug.Log("OnPlayerLeftRoom() " + other.NickName);

        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Lobby");
    }

    public void StartMatch()
    {
        ballingame = GameObject.FindGameObjectWithTag("Ball");
        if (ballingame == null)
        {
            ballingame = PhotonNetwork.Instantiate(Ball.name, transform.position, Quaternion.identity, 0);
        }
        photonView.RPC("RPC_StartMatch", RpcTarget.AllBuffered);
    }
    [PunRPC]
    public void RPC_StartMatch()
    { 
        Reset();
        StartCoroutine(ResetBall());
    }
    [PunRPC]
    void EndMatch()
    {
        Rigidbody rb = ballingame.GetComponent<Rigidbody>();
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        rb.isKinematic = true;
    }

    private void Reset()
    {
        scorep1 = 0;
        scorep2 = 0;
        textp1.text = scorep1.ToString();
        textp2.text = scorep2.ToString();
    }

    private IEnumerator ResetBall()
    {
        ballingame = GameObject.FindGameObjectWithTag("Ball");
        if (ballingame != null)
        {
            Rigidbody rb = ballingame.GetComponent<Rigidbody>();
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
            Vector3 randposition = new Vector3(Random.value * 8, 10, Random.value * 6);
            yield return new WaitForSecondsRealtime(2);

            ballingame.transform.position = randposition;
            yield return new WaitForSecondsRealtime(2);

            rb.isKinematic = false;
        }
    }

    [PunRPC]
    void RPC_Scorep1()
    {
        scorep1++;
        textp1.text = scorep1.ToString();
       
        if (scorep1 >= 7)
        {
            photonView.RPC("EndMatch", RpcTarget.AllBuffered);
            return;
        }
        StartCoroutine(ResetBall());
    }

    public void Scorep1()
    {
        photonView.RPC("RPC_Scorep1", RpcTarget.AllBuffered);
    }

    public void Scorep2()
    {
        photonView.RPC("RPC_Scorep2", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void RPC_Scorep2()
    {
        scorep2++;
        textp2.text = scorep2.ToString();
        
        if (scorep2 >= 7)
        {
            photonView.RPC("EndMatch", RpcTarget.AllBuffered);
            return;
        }
        StartCoroutine(ResetBall());
    }
}
