using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {
    public enum Event{
        Chase,
        SmallPickup,
        BigPickup,
        Score
    }

    public AudioClip BackgroundSound;
    [Range(0, 1)]
    public float BackgroundVolume = 1.0f;

    public AudioClip ChaseSound;
    [Range(0, 1)]
    public float ChaseVolume = 1.0f;

    public AudioClip SeedSound;
    [Range(0, 1)]
    public float SeedVolume = 1.0f;

    public AudioClip SandwitchSound;
    [Range(0, 1)]
    public float SandwitchVolume = 1.0f;

    public AudioClip ScoreSound;
    [Range(0, 1)]
    public float ScoreVolume = 1.0f;

    string[] names = {"Background", "Chase", "Seed", "Sandwitch", "Score"};

    public List<AudioSource> sources;

    public static AudioManager instance;

    void Awake(){
        if (instance != null && instance != this){
            Destroy(this.gameObject);
        }else{
			instance = this;
		}

        QualitySettings.vSyncCount = 1;
        Application.targetFrameRate = 60;
        
        sources = new List<AudioSource>();



        // System.Type t = typeof(AudioManager);
        // PropertyInfo[] properties = t.GetFields();

        // foreach(PropertyInfo pI in properties) {
        //   Debug.Log(pI.Name + ": " + pI.GetValue(this));
        //   //This returns the value cast (or boxed) to object so you will need to cast it to some other type to find its value.
        // }

        System.Type t = typeof(AudioManager);
        foreach(var name in names){
            var src = gameObject.AddComponent<AudioSource>();
            src.playOnAwake = false;

            var SoundProp = t.GetField(name + "Sound");
            var VolProp = t.GetField(name + "Volume");


            src.clip = (AudioClip)SoundProp.GetValue(this);
            src.volume = (float)VolProp.GetValue(this);
            if(name == "Background" || name == "Chase"){
                src.loop = true;
                src.Play();
            }
            sources.Add(src);
        }
    }

    public static void Emit(Event e){
        if(!instance){
            Debug.LogError("No Instance of AudioManager Exists");
            return;
        }

        AudioSource src = instance.sources[(int)e + 1];

        src.Play();
    }

    public static AudioSource GetSource(Event e){
        if(!instance){
            Debug.LogError("No Instance of AudioManager Exists");
            return null;
        }

        return instance.sources[(int)e + 1];
    }
}