using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.Common;
using System.IO;
using UnityEngine.Networking;
using System;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public Lane[] lanes;
    public static MidiFile midiFile;
    public float songDelayInSeconds = 0f;
    public double marginOfError;    //Seconds Unit
    public int inputDelayInMilliseconds;
    public string fileLocation;
    public float noteTime;
    public float noteSpawnY;
    public float noteTapY;
    public float noteDespawnY
    {
        get
        {
            return noteTapY - (noteSpawnY - noteTapY);
        }
    }



    void Start()
    {
        Instance = this;
        if (Application.streamingAssetsPath.StartsWith("http://") || Application.streamingAssetsPath.StartsWith("https://"))
        {
            StartCoroutine(ReadFromWebsite());
            return;
        }
        
        ReadFromFile();
        //ReadFromFileNone();
    }
    
    void Update()
    {
    }

    private IEnumerator ReadFromWebsite()
    {
        using (UnityWebRequest www = UnityWebRequest.Get(Application.streamingAssetsPath + "/" + fileLocation))
        {
            yield return www.SendWebRequest();
            
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                byte[] results = www.downloadHandler.data;
                using (var stream = new MemoryStream(results))
                {
                    midiFile = MidiFile.Read(stream);
                    GetDataFromMidi();
                }
            }
        }
    }

    private void ReadFromFile()
    {
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + fileLocation);
        GetDataFromMidi();
    }

    private void ReadFromFileNone()
    {
        CreateFileNone();
        midiFile = MidiFile.Read(Application.streamingAssetsPath + "/" + "None.mid");
        GetDataFromMidi();
    }

    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        foreach (var lane in lanes) 
            lane.SetTimeStamps(array);

        Invoke(nameof(StartSong), songDelayInSeconds);
    }

    public void CreateFileNone()
    {
        var midiFile = new MidiFile();
        TempoMap tempoMap = midiFile.GetTempoMap();

        for (int i = 0; i < 10; ++i)
        {
        var trackChunk = new TrackChunk();
        using (var notesManager = trackChunk.ManageNotes())
        {
            NotesCollection notes = notesManager.Notes;
            notes.Add(new Melanchall.DryWetMidi.Interaction.Note(
                0,
                3,
                LengthConverter.ConvertFrom(
                    new MetricTimeSpan(hours: 0, minutes: 0, seconds: 1),
                    0,
                    tempoMap)));

            notes.Add(new Melanchall.DryWetMidi.Interaction.Note(
                0,
                3,
                LengthConverter.ConvertFrom(
                    new MetricTimeSpan(hours: 0, minutes: 0, seconds: 1),
                    0,
                    tempoMap)));
        }
        midiFile.Chunks.Add(trackChunk);
        }

        midiFile.Write(Application.streamingAssetsPath + "/" + "None.mid", true);
    }

    public void StartSong()
    {
        if (audioSource == null) 
        {
            Debug.LogAssertion("audioSource is empty");
            return;
        }

        audioSource.Play();
    }

    public static double GetAudioSourceTime()
    {
        if (Instance == null || Instance.audioSource == null || Instance.audioSource.clip == null) 
        {
            Debug.LogAssertion("audioSource is not setting");
            return -1.0f;
        }

        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }
}
