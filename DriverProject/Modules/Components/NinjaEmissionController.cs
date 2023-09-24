using UnityEngine;
using RoR2;

public class NinjaEmissionController : MonoBehaviour
{
    public float maxEmission = 1f;
    public float smoothSpeed = 3f;

    public SkinnedMeshRenderer bodyRenderer;
    public SkinnedMeshRenderer feetRenderer;

    public TrailRenderer[] trails = new TrailRenderer[0];
    public TrailRenderer[] sprintTrails = new TrailRenderer[0];
    public ParticleSystem[] activeEffects = new ParticleSystem[0];
    public ParticleSystem[] sprintEffects = new ParticleSystem[0];

    public Color baseTrailColor = Color.black;
    public Color activeTrailColor = Color.white;

    public string sprintSoundString = "sfx_thruster_loop";

    private float currentPower;
    private float currentSprintPower;
    private float sprintStopwatch;
    private uint sprintSoundPlayID;
    public CharacterBody characterBody;

    private void Awake()
    {
        this.characterBody = this.GetComponent<CharacterBody>();
    }

    private void FixedUpdate()
    {
        if (this.characterBody)
        {
            this.Simulate();
        }
    }

    private void Simulate()
    {
        if (this.characterBody.inputBank.skill3.down) this.sprintStopwatch = 0.25f;
        else this.sprintStopwatch -= Time.fixedDeltaTime;

        if (!this.characterBody.outOfCombat)
        {
            this.currentPower += Time.fixedDeltaTime * this.smoothSpeed;
        }
        else
        {
            this.currentPower -= Time.fixedDeltaTime * this.smoothSpeed;
        }

        if (this.characterBody.inputBank.skill3.down)
        {
            this.currentSprintPower += Time.fixedDeltaTime * this.smoothSpeed;
        }
        else
        {
            this.currentSprintPower -= Time.fixedDeltaTime * this.smoothSpeed;
        }

        this.currentPower = Mathf.Clamp(this.currentPower, 0f, this.maxEmission);
        this.currentSprintPower = Mathf.Clamp(this.currentSprintPower, 0f, this.maxEmission);

        if (this.bodyRenderer)
        {
            this.bodyRenderer.material.SetFloat("_EmPower", this.currentPower);
        }

        if (this.feetRenderer)
        {
            this.feetRenderer.material.SetFloat("_EmPower", this.currentSprintPower);
        }

        if (this.trails.Length > 0)
        {
            for (int i = 0; i < this.trails.Length; i++)
            {
                if (this.trails[i])
                {
                    Color col = this.trails[i].startColor;
                    float t = Util.Remap(this.currentPower, 0f, this.maxEmission, 0f, 1f);

                    col.r = Mathf.Lerp(this.baseTrailColor.r, this.activeTrailColor.r, t);
                    col.g = Mathf.Lerp(this.baseTrailColor.g, this.activeTrailColor.g, t);
                    col.b = Mathf.Lerp(this.baseTrailColor.b, this.activeTrailColor.b, t);

                    this.trails[i].startColor = col;
                }
            }
        }

        if (this.sprintTrails.Length > 0)
        {
            for (int i = 0; i < this.sprintTrails.Length; i++)
            {
                if (this.sprintTrails[i])
                {
                    Color col = this.sprintTrails[i].startColor;
                    float t = Util.Remap(this.currentSprintPower, 0f, this.maxEmission, 0f, 1f);

                    col.r = Mathf.Lerp(this.baseTrailColor.r, this.activeTrailColor.r, t);
                    col.g = Mathf.Lerp(this.baseTrailColor.g, this.activeTrailColor.g, t);
                    col.b = Mathf.Lerp(this.baseTrailColor.b, this.activeTrailColor.b, t);

                    this.sprintTrails[i].startColor = col;
                }
            }
        }

        if (this.currentPower >= this.maxEmission)
        {
            if (this.activeEffects.Length > 0)
            {
                for (int i = 0; i < this.activeEffects.Length; i++)
                {
                    if (this.activeEffects[i])
                    {
                        if (!this.activeEffects[i].isPlaying) this.activeEffects[i].Play();
                    }
                }
            }
        }
        else
        {
            for (int i = 0; i < this.activeEffects.Length; i++)
            {
                if (this.activeEffects[i])
                {
                    if (this.activeEffects[i].isPlaying) this.activeEffects[i].Stop();
                }
            }
        }

        if (this.sprintStopwatch >= 0f)
        {
            if (this.sprintSoundPlayID == 0) this.sprintSoundPlayID = Util.PlaySound(this.sprintSoundString, this.gameObject);

            if (this.sprintEffects.Length > 0)
            {
                for (int i = 0; i < this.sprintEffects.Length; i++)
                {
                    if (this.sprintEffects[i])
                    {
                        if (!this.sprintEffects[i].isPlaying) this.sprintEffects[i].Play();
                    }
                }
            }
        }
        else
        {
            if (this.sprintSoundPlayID == 0)
            {
                AkSoundEngine.StopPlayingID(this.sprintSoundPlayID);
                this.sprintSoundPlayID = 0;
            }

            for (int i = 0; i < this.sprintEffects.Length; i++)
            {
                if (this.sprintEffects[i])
                {
                    if (this.sprintEffects[i].isPlaying) this.sprintEffects[i].Stop();
                }
            }
        }
    }
}