using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField]
    private string menuMusic;
    [SerializeField]
    private string gameMusic;
    [SerializeField]
    private string ambientSounds;

    private static FMOD.Studio.EventInstance Music;

    void Start()
    {
        Music = FMODUnity.RuntimeManager.CreateInstance("event:/" + menuMusic);
        Music.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        Music.start();
        Music.release();
        DontDestroyOnLoad(this.gameObject);
    }

    public void ChangeMusic()
    {
        Music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        Music = FMODUnity.RuntimeManager.CreateInstance("event:/" + gameMusic);
        Music.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        Music.start();
        Music.release();
    }

    public void UseAmbient()
    {
        Music.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        Music = FMODUnity.RuntimeManager.CreateInstance("event:/" + ambientSounds);
        Music.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        Music.start();
        Music.release();
    }
}
