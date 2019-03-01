/* This file is part of INDISharp, Copyright © 2014-2015 Ilia Platone <info@iliaplatone.com>.
*
*  INDISharp is free software: you can redistribute it and/or modify
*  it under the terms of the GNU General Public License as published by
*  the Free Software Foundation, either version 3 of the License, or
*  (at your option) any later version.
*  
*  INDISharp is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU General Public License for more details.
*  
*  You should have received a copy of the GNU General Public License
*  along with INDISharp.  If not, see <http://www.gnu.org/licenses/>.
*/

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Xml.Linq;

namespace INDI
{
    public class INDIBaseDevice
	{
	    public bool ConnectionState { get; private set; }
	    public event EventHandler<IsNewSwitchEventArgs> ConnectedChanged;
	    public string Name { get; }
	    public List<TextVector> Texts { get; }
	    public List<NumberVector> Numbers { get; }
	    public List<SwitchVector> Switches { get; }
	    public List<BlobVector> Blobs { get; }
	    public INDIClient Host { get; }

	    public INDIBaseDevice(string n, INDIClient host, bool client = true)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            if (host == null)
                throw new ArgumentException("host cannot be null");
            IsClient = client;
            Host = host;
            Name = n;
            Texts = new List<TextVector>();
            Numbers = new List<NumberVector>();
            Switches = new List<SwitchVector>();
            Blobs = new List<BlobVector>();
            host.IsDelProperty += isDelProperty;
            host.IsNewText += isNewText;
            host.IsNewNumber += isNewNumber;
			host.IsNewSwitch += isNewSwitch;
			host.IsNewBlob += isNewBlob;
			host.IsNewMessage += isNewMessage;
        }

        public virtual void isDelProperty(object sender, IsDelPropertyEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            if (e.Device == Name)
            {
                Switches.Remove(GetSwitchVector(e.Vector));
                Blobs.Remove(GetBlobVector(e.Vector));
                Numbers.Remove(GetNumberVector(e.Vector));
                Texts.Remove(GetTextVector(e.Vector));
            }
        }

        public virtual void isNewText(object sender, IsNewTextEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            if (e.Device == Name)
            {
                var v = GetTextVector(e.Vector.Name);
                if (v == null)
                    AddTextVector(e.Vector);
            }
        }

        public virtual void isNewNumber(object sender, IsNewNumberEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            if (e.Device == Name)
            {
                var v = GetNumberVector(e.Vector.Name);
                if (v == null)
                    AddNumberVector(e.Vector);
            }
        }

        public virtual void isNewSwitch(object sender, IsNewSwitchEventArgs e)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            if (e.Device == Name)
            {
                var v = GetSwitchVector(e.Vector.Name);
                if (v == null)
					AddSwitchVector(e.Vector);
				if(e.Vector.Name == "CONNECTION" && e.Vector.Values[0].Value != ConnectionState)
				{
					ConnectionState = e.Vector.Values[0].Value;
					ConnectedChanged?.Invoke(this, e);
				}
            }
		}

		public virtual void isNewBlob(object sender, IsNewBlobEventArgs e)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
			if (e.Device == Name)
			{
				BlobVector v = GetBlobVector(e.Vector.Name);
				if (v == null)
					AddBlobVector(e.Vector);
			}
		}

		public virtual void isNewMessage(object sender, IsNewMessageEventArgs e)
		{
			Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
			Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
			if (e.Device == Name)
			{
				Console.WriteLine (Name + ": " + e.Message);
			}
		}

        public string EnableBLOB(bool enable)
        {
            string ret =
                new XElement("enableBLOB", new XAttribute("device", Name), (enable ? "Also" : "Never")).ToString();
            Host.OutputString = ret;
            return ret;
        }

        public void AddTextVector(TextVector v)
        {
            TextVector vector = GetTextVector(v.Name);
            if (vector != null)
            {
                GetTextVector(v.Name).Change(v);
                return;
            }
            Texts.Add(v);
        }

        public void AddSwitchVector(SwitchVector v)
        {
            SwitchVector vector = GetSwitchVector(v.Name);
            if (vector != null)
            {
                GetSwitchVector(v.Name).Change(v);
                return;
            }
            Switches.Add(v);
        }

        public void AddNumberVector(NumberVector v)
        {
            NumberVector vector = GetNumberVector(v.Name);
            if (vector != null)
            {
                GetNumberVector(v.Name).Change(v);
                return;
            }
            Numbers.Add(v);
        }

        public void AddBlobVector(BlobVector v)
        {
            BlobVector vector = GetBlobVector(v.Name);
            if (vector != null)
            {
                GetBlobVector(v.Name).Change(v);
                return;
            }
            Blobs.Add(v);
        }

        public List<string> GetGroups()
        {
            var ret = new List<string>();
            foreach (var switchVector in Host.GetDevice(Name).Switches)
            {
                if (!ret.Contains(switchVector.Group))
                    ret.Add(switchVector.Group);
            }
            foreach (var numberVector in Host.GetDevice(Name).Numbers)
            {
                if (!ret.Contains(numberVector.Group))
                    ret.Add(numberVector.Group);
            }
            foreach (var textVector in Host.GetDevice(Name).Texts)
            {
                if (!ret.Contains(textVector.Group))
                    ret.Add(textVector.Group);
            }
            foreach (var blobVector in Host.GetDevice(Name).Blobs)
            {
                if (!ret.Contains(blobVector.Group))
                    ret.Add(blobVector.Group);
            }
            return ret;
        }

        public SwitchVector GetSwitchVector(string name)
        {
			INDIDevice d = Host.GetDevice(Name);
            foreach (SwitchVector s in d.Switches)
                if (s.Name == name && name != "")
                    return s;
            return null;
        }

        public NumberVector GetNumberVector(string name)
		{
			INDIDevice d = Host.GetDevice(Name);
			foreach (NumberVector s in d.Numbers)
                if (s.Name == name && name != "")
                    return s;
            return null;
        }

        public TextVector GetTextVector(string name)
        {
            foreach (TextVector s in Host.GetDevice(Name).Texts)
                if (s.Name == name && name != "")
                    return s;
            return null;
        }

        public BlobVector GetBlobVector(string name)
        {
            foreach (BlobVector s in Host.GetDevice(Name).Blobs)
                if (s.Name == name && name != "")
                    return s;
            return null;
        }

        public INDINumber GetNumber(string vector, string name)
        {
            NumberVector v = GetNumberVector(vector);
            if (v != null)
            {
                for (int i = 0; i < v.Values.Count; i++)
                {
                    if (v.Values[i].Name == name)
                        return v.Values[i];
                }
            }
            return null;
        }

        public INDIText GetText(string vector, string name)
        {
            TextVector v = GetTextVector(vector);
            if (v != null)
            {
                for (int i = 0; i < v.Values.Count; i++)
                {
                    if (v.Values[i].Name == name)
                        return v.Values[i];
                }
            }
            return null;
        }

        public INDISwitch GetSwitch(string vector, string name)
        {
            SwitchVector v = GetSwitchVector(vector);
            if (v != null)
            {
                for (int i = 0; i < v.Values.Count; i++)
                {
                    if (v.Values[i].Name == name)
                        return v.Values[i];
                }
            }
            return null;
        }

        public INDIBlob GetBlob(string vector, string name)
        {
            BlobVector v = GetBlobVector(vector);
            if (v != null)
            {
                for (int i = 0; i < v.Values.Count; i++)
                {
                    if (v.Values[i].Name == name)
                        return v.Values[i];
                }
            }
            return null;
        }

        public string SetVector(string name, byte[][] blobs, bool[] switches, string[] texts, double[] numbers)
        {
            if (blobs != null)
                return SetBlobVector(name, blobs);
            if (switches != null)
                return SetSwitchVector(name, switches);
            if (texts != null)
                return SetTextVector(name, texts);
            if (numbers != null)
                return SetNumberVector(name, numbers);
            return "";
        }

        public string SetNumber(string vector, string name, double value)
        {
            NumberVector v = GetNumberVector(vector);
            if (v == null)
                throw new ArgumentException();
            double[] values = new double[v.Values.Count];
            for (int i = 0; i < values.Length; i++)
            {
                if (v.Values[i].Name == name)
                    values[i] = value;
                else
                    values[i] = v.Values[i].Value;
            }
            return SetNumberVector(vector, values);
        }

        public string SetText(string vector, string name, string value)
        {
            TextVector v = GetTextVector(vector);
            if (v == null)
                throw new ArgumentException();
            string[] values = new string[v.Values.Count];
            for (int i = 0; i < values.Length; i++)
            {
                if (v.Values[i].Name == name)
                    values[i] = value;
                else
                    values[i] = v.Values[i].Value;
            }
            return SetTextVector(vector, values);
        }

        public string SetSwitch(string vector, string name, bool value)
        {
            SwitchVector v = GetSwitchVector(vector);
            if (v == null)
                throw new ArgumentException();
            bool[] values = new bool[v.Values.Count];
            for (int i = 0; i < values.Length; i++)
            {
                if (v.Values[i].Name == name)
                {
                    values[i] = value;
                }
                else
                {
                    if (v.Rule == "OneOfMany")
                    {
                        values[i] = !value;
                    }
                    else
                    {
                        values[i] = v.Values[i].Value;
                    }
                }
            }
            return SetSwitchVector(vector, values);
        }

        public string SetBlob(string vector, string name, byte[] value)
        {
            BlobVector v = GetBlobVector(vector);
            if (v == null)
                throw new ArgumentException();
            byte[][] values = new byte[v.Values.Count][];
            for (int i = 0; i < values.Length; i++)
            {
                if (v.Values[i].Name == name)
                    values[i] = value;
                else
                    values[i] = v.Values[i].Value;
            }
            return SetBlobVector(vector, values);
        }

        public string SetNumberVector(string vector, double[] values)
        {
            if (GetNumberVector(vector) == null)
                throw new ArgumentException();
            var items = new XElement[values.Length];
            for (var i = 0; i < GetNumberVector(vector).Values.Count; i++)
            {
                items[i] = new XElement("oneNumber", new XAttribute("name", GetNumberVector(vector).Values[i].Name), values[i].ToString().Replace(",", "."));
                GetNumberVector(vector).Values[i].Value = values[i];
            }
            string ret =
                new XElement("newNumberVector",
                    new XAttribute("device", Name),
                    new XAttribute("name", vector),
                    items).ToString();
            Host.OutputString = ret;
            if (IsClient)
                DefineNumbers(vector);
            return ret;
        }

        public string SetTextVector(string vector, string[] values)
        {
            if (GetTextVector(vector) == null)
                throw new ArgumentException();
            XElement[] items = new XElement[values.Length];
            for (int i = 0; i < GetTextVector(vector).Values.Count; i++)
            {
                items[i] = new XElement("oneText", new XAttribute("name", GetTextVector(vector).Values[i].Name), values[i]);
                GetTextVector(vector).Values[i].Value = values[i];
            }
            string ret =
                new XElement("newTextVector",
                    new XAttribute("device", Name),
                    new XAttribute("name", vector),
                    items).ToString();
            Host.OutputString = ret;
            if (IsClient)
                DefineTexts(vector);
            return ret;
        }

        public string SetSwitchVector(string vector, int index)
        {
            if (GetSwitchVector(vector) == null)
                throw new ArgumentException();
            XElement[] items = new XElement[GetSwitchVector(vector).Values.Count];
            for (int i = 0; i < GetSwitchVector(vector).Values.Count; i++)
            {
                items[i] = new XElement("oneSwitch", new XAttribute("name", GetSwitchVector(vector).Values[i].Name), (i == index ? "On" : "Off"));
                GetSwitchVector(vector).Values[i].Value = (i == index);
            }
            string ret =
                new XElement("newSwitchVector",
                    new XAttribute("device", Name),
                    new XAttribute("name", vector),
                    items).ToString();
            Host.OutputString = ret;
            if (IsClient)
                DefineSwitches(vector);
            return ret;
        }

        public string SetSwitchVector(string vector, bool[] values)
        {
            if (GetSwitchVector(vector) == null)
                throw new ArgumentException();
            XElement[] items = new XElement[values.Length];
            for (int i = 0; i < GetSwitchVector(vector).Values.Count; i++)
            {
                items[i] = new XElement("oneSwitch", new XAttribute("name", GetSwitchVector(vector).Values[i].Name), (values[i] ? "On" : "Off"));
                GetSwitchVector(vector).Values[i].Value = values[i];
            }
            string ret =
                new XElement("newSwitchVector",
                    new XAttribute("device", Name),
                    new XAttribute("name", vector),
                    items).ToString();
            Host.OutputString = ret;
            return ret;
        }

        public string SetBlobVector(string vector, byte[][] values)
        {
            if (GetBlobVector(vector) == null)
                throw new ArgumentException();
            XElement[] items = new XElement[values.Length];
            for (int i = 0; i < GetBlobVector(vector).Values.Count; i++)
            {
                string data = Convert.ToBase64String(values[i]);
                items[i] = new XElement("oneBlob", new XAttribute("name", GetBlobVector(vector).Values[i].Name), new XAttribute("format", GetBlobVector(vector).Values[i].Format), new XAttribute("size", data.Length), data);
                GetBlobVector(vector).Values[i].Value = values[i];
            }
            string ret =
                new XElement("newBlobVector",
                    new XAttribute("device", Name),
                    new XAttribute("name", vector),
                    items).ToString();
            Host.OutputString = ret;
            return ret;
        }

        public string QueryProperties(string vector = "")
        {
            string ret =
                new XElement("getProperties",
                    new XAttribute("device", Name),
                    new XAttribute("name", vector),
                    new XAttribute("version", "1.7")).ToString();
            Host.OutputString = ret;
            return ret;
        }

        public string DefineProperties(string vector = "")
        {
            string ret = "";
            ret += DefineNumbers(vector);
            ret += DefineSwitches(vector);
            ret += DefineTexts(vector);
            ret += DefineBlobs(vector);
            return ret;
        }

        public string DefineNumbers(string name = "")
        {
            string ret = "";
            foreach (NumberVector vector in Numbers)
            {
                if ((name != "" && vector.Name == name) || name == "")
                {
                    XElement[] items = new XElement[vector.Values.Count];
                    for (int i = 0; i < vector.Values.Count; i++)
                    {
                        items[i] = new XElement("defNumber",
                            new XAttribute("label", vector.Values[i].Label),
                            new XAttribute("name", vector.Values[i].Name),
                            new XAttribute("format", vector.Values[i].Format),
                            new XAttribute("min", vector.Values[i].Min),
                            new XAttribute("max", vector.Values[i].Max),
                            new XAttribute("step", vector.Values[i].Step),
                            vector.Values[i].Value.ToString());
                    }
                    ret +=
                        new XElement("defNumberVector",
                            new XAttribute("device", vector.Device),
                            new XAttribute("name", vector.Name),
                            new XAttribute("label", vector.Label),
                            new XAttribute("group", vector.Group),
                            new XAttribute("perm", vector.Permission),
                            items).ToString();
                }
            }
            Host.OutputString = ret;
            return ret;
        }

        public string DefineTexts(string name = "")
        {
            string ret = "";
            foreach (TextVector vector in Texts)
            {
                if ((name != "" && vector.Name == name) || name == "")
                {
                    XElement[] items = new XElement[vector.Values.Count];
                    for (int i = 0; i < vector.Values.Count; i++)
                    {
                        items[i] = new XElement("defText",
                            new XAttribute("label", vector.Values[i].Label),
                            new XAttribute("name", vector.Values[i].Name),
                            vector.Values[i].Value);
                    }
                    ret +=
                        new XElement("defTextVector",
                            new XAttribute("device", vector.Device),
                            new XAttribute("name", vector.Name),
                            new XAttribute("label", vector.Label),
                            new XAttribute("group", vector.Group),
                            new XAttribute("perm", vector.Permission),
                            items).ToString();
                }
            }
            Host.OutputString = ret;
            return ret;
        }

        public string DefineSwitches(string name = "")
        {
            string ret = "";
            foreach (SwitchVector vector in Switches)
            {
                if ((name != "" && vector.Name == name) || name == "")
                {
                    XElement[] items = new XElement[vector.Values.Count];
                    for (int i = 0; i < vector.Values.Count; i++)
                    {
                        items[i] = new XElement("defSwitch",
                            new XAttribute("label", vector.Values[i].Label),
                            new XAttribute("name", vector.Values[i].Name),
                            (vector.Values[i].Value ? "On" : "Off"));
                    }
                    ret +=
                        new XElement("defSwitchVector",
                            new XAttribute("device", vector.Device),
                            new XAttribute("name", vector.Name),
                            new XAttribute("label", vector.Label),
                            new XAttribute("group", vector.Group),
                            new XAttribute("rule", vector.Rule),
                            new XAttribute("perm", vector.Permission),
                            items).ToString();
                }
            }
            Host.OutputString = ret;
            return ret;
        }

        public string DefineBlobs(string name = "")
        {
            string ret = "";
            foreach (BlobVector vector in Blobs)
            {
                if ((name != "" && vector.Name == name) || name == "")
                {
                    XElement[] items = new XElement[vector.Values.Count];
                    for (int i = 0; i < vector.Values.Count; i++)
                    {
                        items[i] = new XElement("defBlob",
                            new XAttribute("label", vector.Values[i].Label),
                            new XAttribute("name", vector.Values[i].Name),
                            new XAttribute("format", vector.Values[i].Format),
                            Convert.ToBase64String(vector.Values[i].Value));
                    }
                    ret =
                        new XElement("defBlobVector",
                            new XAttribute("device", vector.Device),
                            new XAttribute("name", vector.Name),
                            new XAttribute("label", vector.Label),
                            new XAttribute("group", vector.Group),
                            new XAttribute("perm", vector.Permission),
                            items).ToString();
                }
            }
            Host.OutputString = ret;
            return ret;
        }
        public bool IsClient { get; internal set; }
    }
}

