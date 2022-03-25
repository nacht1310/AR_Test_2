using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using M2MqttUnity;
using uPLibrary.Networking.M2Mqtt;
using uPLibrary.Networking.M2Mqtt.Messages;

public class value_publish_script : M2MqttUnity.M2MqttUnityClient
{
    private int numberOfSubcribeText = 2;
    private Text[] subcribeText;
    private InputField[] publishInput;
    private string[] messageSubcribe;
    private string messagePublish;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        subcribeText = new Text[numberOfSubcribeText];
        for (int i = 0; i<= numberOfSubcribeText-1; i++)
        {
            string name = "subcribeText" + (i+1).ToString();
            subcribeText[i] = GameObject.Find(name).GetComponent<Text>();
            subcribeText[i].text = "";
        }
        publishInput = new InputField[numberOfSubcribeText];
        for (int i = 0; i <= numberOfSubcribeText - 1; i++)
        {
            string name = "publishInput" + (i+1).ToString();
            publishInput[i] = GameObject.Find(name).GetComponent<InputField>();
        }
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }
    protected override void OnConnected()
    {
        base.OnConnected();
        Debug.Log("Connected");
    }
    protected override void SubscribeTopics()
    {
        client.Subscribe(new string[] { "DesktopAppPublish" }, new byte[] { MqttMsgBase.QOS_LEVEL_AT_LEAST_ONCE });
    }
    protected override void DecodeMessage(string topic, byte[] message)
    {
        messageSubcribe = System.Text.Encoding.UTF8.GetString(message).Split(':');
        for(int i=0; i<= numberOfSubcribeText-1; i++)
        {
            if(messageSubcribe[0] == "Value " + (i+1).ToString())
            {
                subcribeText[i].text = messageSubcribe[1];
            }
        }
    }
    public void Publish_Messages(Button button)
    {
       for(int i = 0; i <= numberOfSubcribeText-1; i++)
       {
            if(button.name == "publishButton" + (i+1).ToString())
            {
                messagePublish = "Value " + (i + 1).ToString() + ": " + publishInput[i].text;
                client.Publish("ARAppPublish", System.Text.Encoding.UTF8.GetBytes(messagePublish), MqttMsgBase.QOS_LEVEL_EXACTLY_ONCE, false);
                //Debug.Log("Button " + (i + 1).ToString() + " is clicked");
                Debug.Log("Publish Button is clicked");
                Debug.Log(messagePublish);
            }           
        }
    }
}
