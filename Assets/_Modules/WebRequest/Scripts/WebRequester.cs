using TMPro;
using UnityEngine;
using Cysharp.Threading.Tasks;
using UnityEngine.Networking;
using Newtonsoft.Json;

class PlayerData
{
    [JsonProperty("playerName")]
    public string PlayerName { get; set; }
    
    [JsonProperty("playerHealth")]
    public int PlayerHealth { get; set; }
    
    [JsonProperty("score")]
    public int Score { get; set; }
    
    [JsonProperty("playerPosition")]
    public Vector3 PlayerPosition  { get; set; }
}

public class WebRequester : MonoBehaviour
{
    static string API_URL( string route ) => "http://localhost:8000/" + route;
    [SerializeField] TextMeshProUGUI _status, _name, _health, _pos, _score;

    PlayerData _player;
    
    async void Start()
    {
        await FetchPlayerData();
    }

    async UniTask FetchPlayerData()
    {
        _status.text = "<color=yellow>Fetching...";
        using UnityWebRequest req = UnityWebRequest.Get( API_URL("data") );

        await req.SendWebRequest();

        if ( req.result != UnityWebRequest.Result.Success ) {
            _status.text = "<color=red>" + req.error;
            Debug.LogError( req.error );
            return;
        }
        
        _status.text = "<color=green>OK!";
        UpdatePlayer( req.downloadHandler.text );
    }

    void UpdatePlayer( string body )
    {
        try {
            _player = JsonConvert.DeserializeObject<PlayerData>( body );
        } catch ( System.Exception e ) {
            Debug.LogError( e.Message );
        }
    }

    void UpdateText()
    {
        if ( _player == null) return;
        
        _name.text = _player.PlayerName;
        _health.text = "health: " + _player.PlayerHealth.ToString();
        _pos.text = "pos: " + _player.PlayerPosition.ToString();
        _score.text = "score: " + _player.Score.ToString();
    }
    
    async UniTask SaveData()
    {
        _status.text = "<color=yellow>Saving...";

        string body = JsonConvert.SerializeObject( _player, settings: new() {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.Indented
        });

        using UnityWebRequest req = UnityWebRequest.Put( API_URL("data"), body );

        await req.SendWebRequest();
        if ( req.result != UnityWebRequest.Result.Success ) {
            _status.text = "<color=red>" + req.error;
            Debug.LogError( req.error );
            return;
        }
        
        _status.text = "<color=green>OK!";
        UpdatePlayer( req.downloadHandler.text );
    }
    
    void Update()
    {
        if ( Input.GetKeyDown( KeyCode.Space ) )
            SaveData().Forget();

        // controls
        var mov = (Vector3.up * Input.GetAxisRaw( "Vertical" ) + Vector3.right * Input.GetAxisRaw( "Horizontal" )).normalized;
        _player.PlayerPosition += mov;

        if ( Input.GetKeyDown( KeyCode.K ) )
            _player.Score++;
        if ( Input.GetKeyDown( KeyCode.J ) )
            _player.Score--;

        if ( Input.GetKeyDown( KeyCode.I ) )
            _player.PlayerHealth++;
        if ( Input.GetKeyDown( KeyCode.U ) )
            _player.PlayerHealth--;

        UpdateText();
    }

}
