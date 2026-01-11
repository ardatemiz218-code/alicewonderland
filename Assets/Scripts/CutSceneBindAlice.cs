using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutsceneBindAlice : MonoBehaviour
{
    public PlayableDirector director;
    public Animator aliceAnimator; // Player(Alice) üzerindeki Animator

    void Awake()
    {
        if (!director) director = GetComponent<PlayableDirector>();
        Bind();
    }

    public void Bind()
    {
        if (!director || !director.playableAsset) return;

        // Timeline içindeki AnimationTrack'i bul ve Alice Animator'a bağla
        var timeline = director.playableAsset as TimelineAsset;
        if (timeline == null) return;

        foreach (var track in timeline.GetOutputTracks())
        {
            if (track is AnimationTrack)
            {
                if (aliceAnimator != null)
                    director.SetGenericBinding(track, aliceAnimator);
            }
        }
    }
}
