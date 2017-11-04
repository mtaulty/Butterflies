using UnityEngine;
using HoloToolkit.Unity.InputModule;

public class ButterflyController : MonoBehaviour, IFocusable, ISpeechHandler, IInputClickHandler
{
  #region ALREADY_HERE
  enum State
  {
    Created,
    Flying,
    Resting
  }
  private void Start()
  {
    this.currentState = State.Created;
  }
  public void Fly()
  {
    if (this.currentState == State.Created)
    {
      this.currentState = State.Flying;

      var currentForward = this.gameObject.transform.forward;
      currentForward.Normalize();

      this.gameObject.transform.forward *= -1;

      this.gameObject.GetComponent<Rigidbody>().velocity = currentForward;

      var flyClip = this.ChildAnimation.GetClip("Fly");
      this.ChildAnimation.clip = flyClip;
      this.ChildAnimation.Play();
    }
  }
  public void Rest()
  {
    if (this.currentState == State.Flying)
    {
      this.gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
      this.Flap();
      this.currentState = State.Resting;
    }
  }
  Animation ChildAnimation
  {
    get
    {
      return(this.gameObject.transform.GetChild(0).gameObject.GetComponent<Animation>());
    }
  }
  private void OnCollisionEnter(Collision collision)
  {
    if (this.currentState == State.Flying)
    {
      var newDirection = Vector3.Reflect(this.gameObject.transform.forward, collision.contacts[0].normal);
      newDirection.Normalize();

      this.gameObject.GetComponent<Rigidbody>().velocity = newDirection;

      this.GetComponent<AudioSource>().Play();
    }
  }
  public void Flap()
  {
    var flapClip = this.ChildAnimation.GetClip("Flap");
    this.ChildAnimation.clip = flapClip;
    this.ChildAnimation.Play();
  }


  State currentState;
  #endregion


  public void OnFocusEnter()
  {
    if (this.currentState != State.Flying)
    {
      this.Flap();
    }
  }

  public void OnFocusExit()
  {
    if (this.currentState != State.Flying)
    {
      this.ChildAnimation.Stop();
    }
  }

  public void OnSpeechKeywordRecognized(SpeechEventData eventData)
  {
    if ((this.currentState == State.Created) &&
      (eventData.RecognizedText.ToLower() == "fly"))
    {
      this.Fly();
    }
  }

  public void OnInputClicked(InputClickedEventData eventData)
  {
    if (this.currentState == State.Resting)
    {
      Destroy(this.gameObject);
    }
  }
}