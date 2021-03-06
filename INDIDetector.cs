﻿/* This file is part of INDISharp, Copyright © 2014-2015 Ilia Platone <info@iliaplatone.com>.
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

namespace INDI
{
    #region Custom Event Argument classes
    public class INDIDetectorBlobEventArgs : EventArgs
	{
		public byte[] Data;
		public string Format;
		public string Name;
        public string Vector;

		public INDIDetectorBlobEventArgs(byte[] data, string name, string vector, string format)
		{
            Vector = vector;
			Name = name;
			Format = format;
			Data = data;
		}
    }
    public class INDIDetectorNumberEventArgs : IsNewNumberEventArgs
    {
        public INDIDetectorNumberType Type;
        public List<INDINumber> Values;
        public INDIDetectorNumberEventArgs(NumberVector vector, string dev) : base(vector, dev)
        {
            Values = vector.Values;
            switch (vector.Name)
            {
                case "SENSOR_INTEGRATION":
                    Type = INDIDetectorNumberType.Integration;
                    break;
				case "SENSOR_ABORT_INTEGRATION":
					Type = INDIDetectorNumberType.AbortIntegration;
                    break;
                case "SENSOR_TEMPERATURE":
                    Type = INDIDetectorNumberType.Temperature;
                    break;
                case "DETECTOR_SETTINGS":
                    Type = INDIDetectorNumberType.Settings;
                    break;

                case "TIME_LST":
                    Type = INDIDetectorNumberType.TimeLst;
                    break;
                case "GEOGRAPHIC_COORD":
                    Type = INDIDetectorNumberType.GeographicCoord;
                    break;
                case "ATMOSPHERE":
                    Type = INDIDetectorNumberType.Atmosphere;
                    break;
                default:
                    Type = INDIDetectorNumberType.Other;
                    break;
            }
        }
    }
    public class INDIDetectorSwitchEventArgs : IsNewSwitchEventArgs
    {
        public INDIDetectorSwitchType Type;
        public List<INDISwitch> Values;
        public INDIDetectorSwitchEventArgs(SwitchVector vector, string dev) : base(vector, dev)
        {
            Values = vector.Values;
            switch (vector.Name)
            {
                case "DETECTOR_COOLER":
                    Type = INDIDetectorSwitchType.Cooler;
                    break;

                case "CONNECTION":
                    Type = INDIDetectorSwitchType.Connection;
                    break;
                case "UPLOAD_MODE":
                    Type = INDIDetectorSwitchType.UploadMode;
                    break;
                default:
                    Type = INDIDetectorSwitchType.Other;
                    break;
            }
        }
    }
    public class INDIDetectorTextEventArgs : IsNewTextEventArgs
    {
        public INDIDetectorTextType Type;
        public List<INDIText> Values;
        public INDIDetectorTextEventArgs(TextVector vector, string dev) : base(vector, dev)
        {
            Values = vector.Values;
            switch (vector.Name)
            {
                case "DETECTOR_CFA":
                    Type = INDIDetectorTextType.Cfa;
                    break;

                case "DEVICE_PORT":
                    Type = INDIDetectorTextType.DevicePort;
                    break;
                case "TIME_UTC":
                    Type = INDIDetectorTextType.TimeUtc;
                    break;
                case "UPLOAD_SETTINGS":
                    Type = INDIDetectorTextType.UploadSettings;
                    break;
                case "ACTIVE_DEVICES":
                    Type = INDIDetectorTextType.ActiveDevices;
                    break;
                default:
                    Type = INDIDetectorTextType.Other;
                    break;
            }
        }
    }
    #endregion
    #region Enums
    public enum INDIDetectorNumberType
    {
        TimeLst,
        GeographicCoord,
        Atmosphere,
        Other,

		Temperature,
		Settings,
        Integration,
		AbortIntegration,
		Trigger,
		Resolution,
    }
    public enum INDIDetectorSwitchType
    {
        Connection,
        UploadMode,
        Other,

        Cooler,
        FrameType,
        Compression,
        FrameReset,
    }
    public enum INDIDetectorTextType
    {
        DevicePort,
        TimeUtc,
        UploadSettings,
        ActiveDevices,
        Other,

        Cfa,
    }
    #endregion
    public class INDIDetector : INDIDevice
    {
        public event EventHandler<INDIDetectorBlobEventArgs> IsNewBlob = null;
        public event EventHandler<INDIDetectorNumberEventArgs> IsNewNumber = null;
        public event EventHandler<INDIDetectorSwitchEventArgs> IsNewSwitch = null;
        public event EventHandler<INDIDetectorTextEventArgs> IsNewText = null;
        #region Constructors / Initialization
        public INDIDetector (string name, INDIClient host, bool client = true)
			: base (name, host, client)
		{
			EnableBLOB (true);
			if (!client) {
				AddNumberVector (new NumberVector (Name, "DETECTOR_CAPTURE", "Capture", "Main Control", "rw", "", new List<INDINumber> {
					new INDINumber ("DETECTOR_CAPTURE_VALUE", "Duration (s)", "%5.2f", 0.05, 10000.0, 0.05, 1.0)
				}));
				AddSwitchVector (new SwitchVector (Name, "DETECTOR_ABORT_CAPTURE", "Expose Abort", "Main Control", "rw", "AtMostOne", new List<INDISwitch> {
					new INDISwitch ("ABORT", "Abort", false)
				}));
				AddNumberVector (new NumberVector (Name, "DETECTOR_TEMPERATURE", "Temperature", "Main Control", "rw", "", new List<INDINumber> {
					new INDINumber ("DETECTOR_TEMPERATURE_VALUE", "Temperature (C)", "%5.2f", -50.0, 50.0, 0.0, 20.0)
				}));
				AddNumberVector (new NumberVector (Name, "DETECTOR_INFO", "Detector Information", "Image Info", "ro", "", new List<INDINumber> {
					new INDINumber ("DETECTOR_SAMPLERATE", "Bandwidth (Hz)", "%18.2f", 0.01, 1.0e+15, 0.01, 1.0e+6),
					new INDINumber ("DETECTOR_FREQUENCY", "Observed frequency (Hz)", "%18.2f", 0.01, 1.0e+15, 0.01, 1.42e+9),
					new INDINumber ("DETECTOR_BITSPERSAMPLE", "Bits per sample", "%3.0f", 1, 64, 1, 8)
				}));
				AddBlobVector (new BlobVector (Name, "DETECTOR", "Capture", "Data Streams", "ro", "", new List<INDIBlob> {
					new INDIBlob ("CONTINUUM", Name + " continuum data", ".fits", new byte[1], 1),
					new INDIBlob ("SPECTRUM", Name + " spectrum data", ".fits", new byte[1], 1),
				}));
                DriverInterface |= DRIVER_INTERFACE.DETECTOR_INTERFACE;
            }
		}
        #endregion

        #region Standard Methods
        public void StartCapture(double duration)
        {
            try
            {
                SetNumber("DETECTOR_CAPTURE", "DETECTOR_CAPTURE_VALUE", duration);
            }
            catch
            {
            }
        }

        public void Abort()
        {
            try
            {
                SetSwitch("DETECTOR_ABORT_CAPTURE", "ABORT", true);
            }
            catch { }
        }

        public override void isNewNumber(object sender, IsNewNumberEventArgs e)
        {
            base.isNewNumber(sender, e);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            try
            {
                if (e.Vector.Device == Name)
                {
                    IsNewNumber?.Invoke(this, new INDIDetectorNumberEventArgs(e.Vector, e.Device));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override void isNewSwitch(object sender, IsNewSwitchEventArgs e)
        {
            base.isNewSwitch(sender, e);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            try
            {
                if (e.Vector.Device == Name)
                {
                    IsNewSwitch?.Invoke(this, new INDIDetectorSwitchEventArgs(e.Vector, e.Device));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override void isNewText(object sender, IsNewTextEventArgs e)
        {
            base.isNewText(sender, e);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            try
            {
                if (e.Vector.Device == Name)
                {
                    IsNewText?.Invoke(this, new INDIDetectorTextEventArgs(e.Vector, e.Device));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public override void isNewBlob(object sender, IsNewBlobEventArgs e)
        {
            base.isNewBlob(sender, e);
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");
            Thread.CurrentThread.CurrentUICulture = new CultureInfo("en-US");
            try
            {
                if (e.Vector.Device == Name)
                {
                    INDIClient caller = (INDIClient)sender;
                    for (int i = 0; i < e.Vector.Values.Count; i++)
                    {
                        Console.WriteLine("Received BLOB " + e.Vector.Values[i].Name + " of size " + e.Vector.Values[i].Size + " from device " + e.Device + "@" + caller.Address + ":" + caller.Port);
                        IsNewBlob?.Invoke(this, new INDIDetectorBlobEventArgs(e.Vector.Values[i].Value, e.Vector.Values[i].Name, e.Vector.Name, e.Vector.Values[0].Format));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

		#region Standard Properties
        public double CaptureTime
        {
            get
            {
                try
                {
                    return GetNumber("DETECTOR_CAPTURE", "DETECTOR_CAPTURE_VALUE").Value;
                }
                catch
                {
                }
                return 0.0;
            }
		}

		public double SampleRate
		{
			get
			{
				try
				{
					return GetNumber("DETECTOR_SETTINGS", "DETECTOR_SAMPLERATE").Value;
				}
				catch
				{
				}
				return 0.0;
			}
			set
			{
				try
				{
					SetNumber("DETECTOR_SETTINGS", "DETECTOR_SAMPLERATE", value);
				}
				catch { }
			}
		}

		public double CenterFrequency
		{
			get
			{
				try
				{
					return GetNumber("DETECTOR_SETTINGS", "DETECTOR_FREQUENCY").Value;
				}
				catch
				{
				}
				return 0.0;
			}
			set
			{
				try
				{
					SetNumber("DETECTOR_SETTINGS", "DETECTOR_FREQUENCY", value);
				}
				catch { }
			}
		}

		public double Bps
		{
			get
			{
				try
				{
					return GetNumber("DETECTOR_INFO", "DETECTOR_BITSPERSAMPLE").Value;
				}
				catch
				{
				}
				return 0.0;
			}
			set
			{
				try
				{
					SetNumber("DETECTOR_INFO", "DETECTOR_BITSPERSAMPLE", value);
				}
				catch { }
			}
		}

        public bool CoolerStarted
        {
            get
            {
                try
                {
                    return GetSwitch("DETECTOR_COOLER", "COOLER_ON").Value;
                }
                catch { }
                return false;
            }
            set
            {
                try
                {
                    SetSwitchVector("DETECTOR_COOLER", value ? 0 : 1);
                }
                catch { }
            }
        }

        public double CoolerPower
        {
            get
            {
                try
                {
                    return GetNumber("DETECTOR_COOLER_POWER", "DETECTOR_COOLER_VALUE").Value;
                }
                catch { }
                return 0;
            }
            set
            {
                try
                {
                    SetNumber("DETECTOR_COOLER_POWER", "DETECTOR_COOLER_VALUE", value);
                }
                catch { }
            }
        }

        public bool CanSetCoolerPower
        {
            get
            {
                try
                {
                    return GetNumberVector("DETECTOR_COOLER_POWER").Permission == "rw";
                }
                catch { }
                return false;
            }
        }

        public double MinCapture
        {
            get
            {
                try
                {
                    return GetNumber("DETECTOR_CAPTURE", "DETECTOR_CAPTURE_VALUE").Min;
                }
                catch
                {
                }
                return 0.0;
            }
        }

        public double MaxCapture
        {
            get
            {
                try
                {
                    return GetNumber("DETECTOR_CAPTURE", "DETECTOR_CAPTURE_VALUE").Max;
                }
                catch
                {
                }
                return 0.0;
            }
        }

        public double CaptureStep
        {
            get
            {
                try
                {
                    return GetNumber("DETECTOR_CAPTURE", "DETECTOR_CAPTURE_VALUE").Step;
                }
                catch
                {
                }
                return 0.0;
            }
        }

        public double DetectorTemperature
        {
            get
            {
                try
                {
                    return GetNumber("DETECTOR_TEMPERATURE", "DETECTOR_TEMPERATURE_VALUE").Value;
                }
                catch
                {
                }
                return 0.0;
            }
            set
            {
                try
                {
                    if (CanSetDetectorTemperature)
                    {
                        SetNumber("DETECTOR_TEMPERATURE", "DETECTOR_TEMPERATURE_VALUE", value);
                    }
                }
                catch
                {
                }
            }
        }

        public bool CanSetDetectorTemperature
        {
            get
            {
                try
                {
                    return (GetNumberVector("DETECTOR_TEMPERATURE").Permission == "rw");
                }
                catch
                {
                }
                return false;
            }
        }
        #endregion
	}
}

