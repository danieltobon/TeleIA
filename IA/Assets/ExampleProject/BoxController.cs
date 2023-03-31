using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Lexone.UnityTwitchChat;

/// <summary>
/// This is an example script that shows some examples on how you could use the information from a Chatter object.
/// </summary>
public class BoxController : MonoBehaviour
{
    public Transform ui;
    public Text nameText;
    public Material mat;
    public Shader sh;
   
    [Header("Badge colors")]
    public Color broadcasterColor;
    public Color moderatorColor;
    public Color vipColor;
    public Color subscriberColor;
    public Color yellowColor;
    public Chatter ch;
    public ChangueCamera cam;

    private void Start()
    {
        cam = GameObject.Find("ChatGPT").GetComponent<ChangueCamera>();
    }
    public void Initialize(Chatter chatter)
    {

        ch = chatter;
        // Change name text to chatter's name.
        // Use displayName if it is "font-safe",
        // meaning that it only contains characters: a-z, A-Z, 0-9, _ (most fonts support these characters)
        // If not "font-safe" then use login name instead, which should always be "font-safe"
        nameText.text = chatter.IsDisplayNameFontSafe() ? chatter.tags.displayName : chatter.login;
        nameText.color = chatter.GetNameColor();
        mat = new Material(sh);
        // Change box color to match chatter's primary badge
        gameObject.GetComponent<MeshRenderer>().material = mat;
        if (chatter.HasBadge("broadcaster"))
            mat.color = broadcasterColor;
        else
        if (chatter.HasBadge("moderator"))
            mat.color = moderatorColor;
        else
        if (chatter.HasBadge("vip"))
            mat.color = vipColor;
        else
        if (chatter.HasBadge("subscriber"))
            mat.color = subscriberColor;
        else
        {
            mat.color =  Random.ColorHSV();
        }
       
        if (chatter.ContainsEmote("33"))
        {
            transform.localScale *= 2;
        }

        // If the chatter's message contained the Kappa emote (emote ID: 25)
        // Then make their box double the size

        
        Destroy(this.gameObject,10);
        // Detach UI from parent so that it doesn't rotate with the box
        ui.SetParent(null);

        // Start the jump logic
        StartCoroutine(JumpLogic());
    }

    private void LateUpdate()
    {
        // Update UI position to be above the box
        ui.position = transform.position + new Vector3(0, 0.5f);
        ui.transform.LookAt(cam.camActive.transform.position);
        ui.transform.rotation *= Quaternion.Euler(0, 180, 0);
    }

    public void move(string direc)
    {
        
        Rigidbody rb = GetComponent<Rigidbody>();
        if (direc == "!move")
        {
            // Add some random initial force
            rb.AddForce(Random.insideUnitCircle * Random.Range(5f, 10f), ForceMode.Impulse);
            rb.AddTorque(Vector3.up, ForceMode.Impulse);
            Debug.Log("Dnaiel dice " + direc);
        }
        Vector3 direction = Vector3.up;
        if (direc == "!move derecha")
        {
            direction = Vector3.left;
            Debug.Log("Entro ");
        }
        if (direc == "!move izquierda")
        {
            direction = Vector3.right;
        }
        if (direc == "!move adelante")
        {
            direction = Vector3.forward;
        }
        if (direc == "!move atras")
        {
            direction = -Vector3.forward;
        }

        Vector3 force = Vector2.up * 7f * direction; // Jump force

        rb.AddForce(direction * Random.Range(1f, 3f), ForceMode.Impulse);
        rb.AddForce(Vector3.up * Random.Range(5f, 10f), ForceMode.Impulse);
        //rb.AddTorque(Vector3.up * 10, ForceMode.Impulse);


    }
    private IEnumerator JumpLogic()
    {

        Rigidbody rb = GetComponent<Rigidbody>();

        // Add some random initial force
        rb.AddForce(Random.insideUnitCircle * Random.Range(5f, 10f), ForceMode.Impulse);
        rb.AddTorque(Vector3.up, ForceMode.Impulse);

        while (true)
        {
            yield return new WaitForSeconds(Random.Range(2f, 6f));


            Vector2 force = Vector3.up * 7f; // Jump force

            rb.AddForce(force, ForceMode.Impulse);
            rb.AddForce(Random.insideUnitCircle * Random.Range(5f, 10f), ForceMode.Impulse);
        }
    }
}