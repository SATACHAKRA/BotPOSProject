using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace POSBot
{
    [Serializable]
    public class GlobalHandler
    {
        public string GetRandomString(List<string> stringList)
        {
            Random random = new Random();
            int idx = random.Next(0, 1000) % stringList.Count;
            return stringList[idx];
        }
        /*public async Task makessml(IDialogContext context, string str)
        {
            var speakStr = str.Replace("<BR/>"," ");
            if( str.IndexOf("POS") != -1)
                speakStr = speakStr.Replace("POS", $"<say-as interpret-as=\"characters\">POS</say-as>");
            if (speakStr.IndexOf("PED") != -1)
                speakStr = speakStr.Replace("PED", $"<say-as interpret-as=\"characters\">PED</say-as>");
            //if (speakStr.IndexOf("EID") != -1)
              //  speakStr = speakStr.Replace("EID", $"<say-as interpret-as=\"characters\">EID</say-as>");

            await context.SayAsync(text:str , speak: $"<speak version=\"1.0\" xml:lang=\"en-US\">\r\n  <voice xml:lang=\"en-US\" xml:gender=\"Female\"><prosody pitch = \"x-high\" rate = \"medium\" volume = \"x-loud\" > {speakStr} </prosody></voice>\r\n</speak>");
            

        }


        public async Task makessml1(IDialogContext context, string str)
        {
            var speakStr = str.Replace("<BR/>", " ");
            if (str.IndexOf("POS") != -1)
                speakStr = speakStr.Replace("POS", $"<say-as interpret-as=\"characters\">POS</say-as>");
            if (speakStr.IndexOf("PED") != -1)
                speakStr = speakStr.Replace("PED", $"<say-as interpret-as=\"characters\">PED</say-as>");
            //if (speakStr.IndexOf("EID") != -1)
            //  speakStr = speakStr.Replace("EID", $"<say-as interpret-as=\"characters\">EID</say-as>");

            await context.SayAsync(text: str, speak: $"<speak version=\"1.0\" xml:lang=\"en-US\">\r\n  <voice xml:lang=\"en-US\" xml:gender=\"Female\"><prosody pitch = \"x-low\" rate = \"slow\" volume = \"x-loud\" > {speakStr} </prosody></voice>\r\n</speak>");

        }*/



        public class Close
        {
            public List<string> HelpMessage = new List<string>
            {
                "Is there any additional help required?",
                "Do you need any additional help?",
                "Can I be of some more help?"
            };
            public List<string> RestartMessage = new List<string>
            {
                "Okay, tell me what can I do for you.",
                "Please, tell me what can I do for you.",
                "I request you to tell me what to do.",
                "Tell me what else do you want."
            };
        }
    public class Confirm
    {
        public List<string> ready = new List<string> { "Yes, I am ready", "No, I am not ready" };
        public List<string> proceed = new List<string> { "Yes, I want to proceed", "No, I want to stop here" };
        public List<string> status = new List<string> { "Yes, It was successful", "No, it was not successful" };
        /*public List<string> readyMessage = new List<string>
        {
            "Are you ready?",
            "Ready to do it?",
            "Ready for this?"
        };
        public List<string> proceedMessage = new List<string>
        {
            "Are you ready to proceed?",
            "Ready to proceed?",
            "Ready to proceed to next step?"
        };*/
        public string Step1 = "Step 1: Click on Manager Menu from POS screen.";
        public string Step2 = "Step 2: Press Support Button.";
        public string Step3 = "Step 3: Again press Support Button.";
        public string Step4 = "Step 4: Enter Password.";
        public string Step5 = "Step 5: Press the Cashless Maintenance Button.";
        public string Step6 = "Step 6: Press Open PED.";
        public string Step7 = "Step 7: Reset the Cashless Device.";
        public string Step8 = "Step 8: Select Manager Menu > Special Functions > Reset Cashless device.";
        public string Step9 = "Step 9: Image will come up: Please be sure there are no cards inserted in the PED before continuing, then Press OK.";
        public string Step10 = "Step 10: If successful the following message will come up: PED communication is working.";
    }
    public class EID
    {
        public List<string> assistance = new List<string> { "Person Merge Assistance", "Person Merge Information", "Global Account Manager" };
        public List<string> level = new List<string> { "Staff level", "Store level" };
        public List<string> support = new List<string> { "I want to see presentation", "I want to contact" };
        public List<string> merge = new List<string>
            {
                "Has a person merge been completed?",
                "Have you completed a person merge?",
                "Is a person merge complete?"
            };
    }
    public class LMS
    {
        public string firstname = "Enter your First Name:";
        public string lastname = "Enter your Last Name:";
        public string phonenumber = "Enter your Phone Number:";
        public string storenumber = "Enter your store Number:";
        public string previouseidtext = "Enter your Previous EID:";
        public string previouseidspeak = "Enter your previous e i d:";
        public string neweidtext = "Enter your New EID:";
        public string neweidspeak = "Enter your new e i d:";
        public string position = "Enter your position:";
        public string freceive = "First name is received as: ";
        public string lreceive = "Last name is received as: ";
        public string phreceive = "Phone number is received as: ";
        public string strreceive = "Store number is received as: ";
        public string ptxtreceive = "Previous EID is received as: ";
        public string pspkreceive = "Previous e i d is received as: ";
        public string ntxtreceive = "New EID is received as: ";
        public string nspkreceive = "New e i d is received as: ";
        public string posreceive = "Position is received as: ";
        public string fcom = "Form fill up completed.";
        public string pfirstname = "Your last entered first name was: ";
        public string plastname = "Your last entered last name was: ";
        public string pphone = "Your last entered phone number was: ";
        public string pstore = "Your last entered store number was: ";
        public string ppetxt = "Your last entered Previous EID was: ";
        public string ppespk = "Your last entered previous e i d was: ";
        public string pnetxt = "Your last entered New EID was: ";
        public string pnespk = "Your last entered new e i d was: ";
        public string ppos = "Your last entered position was: ";
    }
    public class POS
    {
        public string Step1 = "Log into a register and confirm when you are ready for the next step.";
        public string Step2 = "If you do not have a POS ID and password, please have a store manager log in.";
    }
}
}
