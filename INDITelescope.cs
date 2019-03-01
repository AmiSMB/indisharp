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

namespace INDI
{
    #region Custom Event Argument classes
    public class INDITelescopeNumberEventArgs : IsNewNumberEventArgs
    {
        public INDITelescopeNumberType Type;
        public List<INDINumber> Values;
        public INDITelescopeNumberEventArgs(NumberVector vector, string dev) : base(vector, dev)
        {
            Values = vector.Values;
            switch (vector.Name)
            {
                case "EQUATORIAL_COORD":
                    Type = INDITelescopeNumberType.EquatorialCoord;
                    break;
                case "EQUATORIAL_EOD_COORD":
                    Type = INDITelescopeNumberType.EquatorialEodCoord;
                    break;
                case "TARGET_EOD_COORD":
                    Type = INDITelescopeNumberType.TargetEodCoord;
                    break;
                case "HORIZONTAL_COORD":
                    Type = INDITelescopeNumberType.HorizontalCoord;
                    break;
                case "TELESCOPE_TIMED_GUIDE_NS":
                    Type = INDITelescopeNumberType.TimedGuideNS;
                    break;
                case "TELESCOPE_TIMED_GUIDE_WE":
                    Type = INDITelescopeNumberType.TimedGuideWE;
                    break;
                case "TELESCOPE_PARK_POSITION":
                    Type = INDITelescopeNumberType.ParkPosition;
                    break;
                case "TELESCOPE_INFO":
                    Type = INDITelescopeNumberType.Informations;
                    break;

                case "TIME_LST":
                    Type = INDITelescopeNumberType.TimeLst;
                    break;
                case "GEOGRAPHIC_COORD":
                    Type = INDITelescopeNumberType.GeographicCoord;
                    break;
                case "ATMOSPHERE":
                    Type = INDITelescopeNumberType.Atmosphere;
                    break;
                default:
                    Type = INDITelescopeNumberType.Other;
                    break;
            }
        }
    }
    public class INDITelescopeSwitchEventArgs : IsNewSwitchEventArgs
    {
        public INDITelescopeSwitchType Type;
        public List<INDISwitch> Values;
        public INDITelescopeSwitchEventArgs(SwitchVector vector, string dev) : base(vector, dev)
        {
            Values = vector.Values;
            switch (vector.Name)
            {
                case "ON_COORD_SET":
                    Type = INDITelescopeSwitchType.OnCoordSet;
                    break;
                case "TELESCOPE_MOTION_NS":
                    Type = INDITelescopeSwitchType.MotionNS;
                    break;
                case "TELESCOPE_MOTION_WE":
                    Type = INDITelescopeSwitchType.MotionWE;
                    break;
                case "TELESCOPE_SLEW_RATE":
                    Type = INDITelescopeSwitchType.SlewRate;
                    break;
                case "TELESCOPE_PARK":
                    Type = INDITelescopeSwitchType.Park;
                    break;
                case "TELESCOPE_PARK_OPTION":
                    Type = INDITelescopeSwitchType.ParkOption;
                    break;
                case "TELESCOPE_ABORT_MOTION":
                    Type = INDITelescopeSwitchType.AbortMotion;
                    break;
                case "TELESCOPE_TRACK_RATE":
                    Type = INDITelescopeSwitchType.TrackRate;
                    break;
                case "TELESCOPE_PIER_SIDE":
                    Type = INDITelescopeSwitchType.PierSide;
                    break;

                case "CONNECTION":
                    Type = INDITelescopeSwitchType.Connection;
                    break;
                case "UPLOAD_MODE":
                    Type = INDITelescopeSwitchType.UploadMode;
                    break;
                default:
                    Type = INDITelescopeSwitchType.Other;
                    break;
            }
        }
    }
    public class INDITelescopeTextEventArgs : IsNewTextEventArgs
    {
        public INDITelescopeTextType Type;
        public List<INDIText> Values;
        public INDITelescopeTextEventArgs(TextVector vector, string dev) : base(vector, dev)
        {
            Values = vector.Values;
            switch (vector.Name)
            {
                case "DEVICE_PORT":
                    Type = INDITelescopeTextType.DevicePort;
                    break;
                case "TIME_UTC":
                    Type = INDITelescopeTextType.TimeUtc;
                    break;
                case "UPLOAD_SETTINGS":
                    Type = INDITelescopeTextType.UploadSettings;
                    break;
                case "ACTIVE_DEVICES":
                    Type = INDITelescopeTextType.ActiveDevices;
                    break;
                default:
                    Type = INDITelescopeTextType.Other;
                    break;
            }
        }
    }
    #endregion
    #region Enums
    public enum INDITelescopeNumberType
    {
        TimeLst,
        GeographicCoord,
        Atmosphere,
        Other,

        EquatorialCoord,
        EquatorialEodCoord,
        TargetEodCoord,
        HorizontalCoord,
        TimedGuideNS,
        TimedGuideWE,
        ParkPosition,
        Informations,
    }
    public enum INDITelescopeSwitchType
    {
        Connection,
        UploadMode,
        Other,

        OnCoordSet,
        MotionNS,
        MotionWE,
        SlewRate,
        Park,
        ParkOption,
        AbortMotion,
        TrackRate,
        PierSide,
    }
    public enum INDITelescopeTextType
    {
        DevicePort,
        TimeUtc,
        UploadSettings,
        ActiveDevices,
        Other,
    }
    public enum INDICoordSet
    {
        SLEW = 0,
        TRACK,
        SYNC
    };
    public enum INDIDirection
    {
        NORTH = 0,
        SOUTH,
        WEST,
        EAST,
        NONE
    };
    public enum INDIRate
    {
        GUIDE = 0,
        CENTERING,
        FIND,
        MAX
    };
    public enum INDITrackRate
    {
        SIDEREAL = 0,
        SOLAR,
        LUNAR,
        CUSTOM
    };
    #endregion
    public class INDITelescope : INDIDevice
    {
        public event EventHandler<INDITelescopeNumberEventArgs> IsNewNumber;
        public event EventHandler<INDITelescopeSwitchEventArgs> IsNewSwitch;
        public event EventHandler<INDITelescopeTextEventArgs> IsNewText;
        #region Constructors / Initialization
        public INDITelescope(string name, INDIClient host, bool client = true)
            : base(name, host, client)
        {
            if (!client)
            {
                AddNumberVector(new NumberVector(Name, "EQUATORIAL_COORD", "Equatorial astrometric J2000 coordinate", "Main Control", "rw", "", new List<INDINumber>
            {
                new INDINumber("RA", "J2000 RA", "%2.12f", 0.00, 24.0, 0.000000000001, 0.0),
                new INDINumber("DEC", "J2000 Dec", "%2.12f", -90.00, 90.0, 0.000000000001, 0.0)
            }));
                AddNumberVector(new NumberVector(Name, "EQUATORIAL_EOD_COORD", "Equatorial astrometric epoch of date coordinate", "Main Control", "rw", "", new List<INDINumber>
            {
                new INDINumber("RA", "JNow RA", "%2.12f", 0.00, 24.0, 0.000000000001, 0.0),
                new INDINumber("DEC", "JNow Dec", "%2.12f", -90.00, 90.0, 0.000000000001, 0.0)
            }));
                AddNumberVector(new NumberVector(Name, "TARGET_EOD_COORD", "Slew Target", "Main Control", "ro", "Main Control", new List<INDINumber>
            {
                new INDINumber("RA", "JNow RA", "%2.12f", 0.00, 24.0, 0.000000000001, 0.0),
                new INDINumber("DEC", "JNow Dec", "%2.12f", -90.00, 90.0, 0.000000000001, 0.0)
            }));
                AddNumberVector(new NumberVector(Name, "HORIZONTAL_COORD", "topocentric coordinate", "Main Control", "ro", "Main Control", new List<INDINumber>
            {
                new INDINumber("ALT", "Altitude", "%2.12f", 0.00, 90.0, 0.000000000001, 0.0),
                new INDINumber("AZ", "Azimuth", "%3.12f", 0.00, 360.0, 0.000000000001, 0.0)
            }));
                AddSwitchVector(new SwitchVector(Name, "ON_COORD_SET", "Track mode", "Main Control", "rw", "OneOfMany", new List<INDISwitch>
            {
                new INDISwitch("SLEW", "Slew", true),
                new INDISwitch("TRACK", "Track", false),
                new INDISwitch("SYNC", "Sync", false)
            }));
                AddSwitchVector(new SwitchVector(Name, "TELESCOPE_MOTION_NS", "Move telescope north or south", "Motion Control", "rw", "OneOfMany", new List<INDISwitch>
            {
                new INDISwitch("MOTION_NORTH", "Move North", false),
                new INDISwitch("MOTION_SOUTH", "Move South", false)
            }));
                AddSwitchVector(new SwitchVector(Name, "TELESCOPE_MOTION_WE", "Move telescope west or east", "Motion Control", "rw", "OneOfMany", new List<INDISwitch>
            {
                new INDISwitch("MOTION_WEST", "Move West", false),
                new INDISwitch("MOTION_EAST", "Move East", false)
            }));
                AddNumberVector(new NumberVector(Name, "TELESCOPE_TIMED_GUIDE_NS", "Timed guide telescope north or south", "Motion Control", "rw", "", new List<INDINumber>
            {
                new INDINumber("TIMED_GUIDE_N", "Guide North", "%5.2f", 0.00, 10000.0, 0.01, 0.0),
                new INDINumber("TIMED_GUIDE_S", "Guide South", "%2.2f", 0.00, 10000.0, 0.01, 0.0)
            }));
                AddNumberVector(new NumberVector(Name, "TELESCOPE_TIMED_GUIDE_WE", "Timed guide telescope west or east", "Motion Control", "rw", "", new List<INDINumber>
            {
                new INDINumber("TIMED_GUIDE_W", "Guide North", "%5.2f", 0.00, 10000.0, 0.01, 0.0),
                new INDINumber("TIMED_GUIDE_E", "Guide South", "%2.2f", 0.00, 10000.0, 0.01, 0.0)
            }));
                AddSwitchVector(new SwitchVector(Name, "TELESCOPE_SLEW_RATE", "Slew Rate", "Motion Control", "rw", "OneOfMany", new List<INDISwitch>
            {
                new INDISwitch("SLEW_GUIDE", "Guiding Rate", false),
                new INDISwitch("SLEW_CENTERING", "Slow speed", false),
                new INDISwitch("SLEW_FIND", "Medium speed", true),
                new INDISwitch("SLEW_MAX", "Maximum speed", false),
            }));
                AddSwitchVector(new SwitchVector(Name, "TELESCOPE_PARK", "Park and unpark the telescope", "Main Control", "rw", "OneOfMany", new List<INDISwitch>
            {
                new INDISwitch("PARK", "Park the telescope", false),
                new INDISwitch("UNPARK", "Unpark the telescope", false)
            }));
                AddNumberVector(new NumberVector(Name, "TELESCOPE_PARK_POSITION", "Home park position", "Main Control", "ro", "", new List<INDINumber>
            {
                new INDINumber("PARK_RA", "JNow RA", "%2.12f", 0.00, 24.0, 0.000000000001, 0.0),
                new INDINumber("PARK_DEC", "JNow Dec", "%2.12f", -90.00, 90.0, 0.000000000001, 0.0)
            }));
                AddSwitchVector(new SwitchVector(Name, "TELESCOPE_PARK_OPTION", "Park Option", "Main Control", "rw", "OneOfMany", new List<INDISwitch>
            {
                new INDISwitch("PARK_CURRENT", "Use current park position", false),
                new INDISwitch("PARK_DEFAULT", "Use driver's default park position", true),
                new INDISwitch("PARK_WRITE_DATA", "Write park position", false)
            }));
                AddSwitchVector(new SwitchVector(Name, "TELESCOPE_ABORT_MOTION", "Abort", "Main Control", "rw", "AtMostOne", new List<INDISwitch>
            {
                new INDISwitch("ABORT_MOTION", "Stop telescope", false)
            }));
                AddSwitchVector(new SwitchVector(Name, "TELESCOPE_TRACK_RATE", "Track Rate", "Main Control", "rw", "AtMostOne", new List<INDISwitch>
            {
                new INDISwitch("TRACK_SIDEREAL", "Track at sidereal rate", true),
                new INDISwitch("TRACK_SOLAR", "Track at solar rate", false),
                new INDISwitch("TRACK_LUNAR", "Track at lunar rate", false),
                new INDISwitch("TRACK_CUSTOM", "Track at custom rate", false),
            }));
                AddNumberVector(new NumberVector(Name, "TELESCOPE_INFO", "Telescope informtations", "Telescope Info", "ro", "Informations", new List<INDINumber>
            {
                new INDINumber("TELESCOPE_APERTURE", "Telescope aperture", "%5.2f", 0.00, 10000.0, 0.01, 200.0),
                new INDINumber("TELESCOPE_FOCAL_LENGTH", "Telescope focal length", "%5.2f", 0.00, 10000.0, 0.01, 200.0),
                new INDINumber("GUIDER_APERTURE", "Guide telescope aperture", "%5.2f", 0.00, 10000.0, 0.01, 200.0),
                new INDINumber("GUIDER_FOCAL_LENGTH", "Guide telescope focal length", "%5.2f", 0.00, 10000.0, 0.01, 200.0),
            }));
                AddSwitchVector(new SwitchVector(Name, "TELESCOPE_PIER_SIDE", "Pier Side", "Motion Control", "rw", "OneOfMany", new List<INDISwitch>
            {
                new INDISwitch("PIER_EAST", "Pointing West", false),
                new INDISwitch("PIER_WEST", "Pointing East", false)
            }));
                DriverInterface |= DRIVER_INTERFACE.TELESCOPE_INTERFACE;
            }
        }
        #endregion

        #region Standard Methods
        public void Move(INDIDirection direction)
        {
            try
            {
                string vector = "TELESCOPE_MOTION_";
                string number = "MOTION_";
                switch (direction)
                {
                    case INDIDirection.NORTH:
                        vector += "NS";
                        number += "N";
                        break;
                    case INDIDirection.SOUTH:
                        vector += "NS";
                        number += "S";
                        break;
                    case INDIDirection.WEST:
                        vector += "WE";
                        number += "W";
                        break;
                    case INDIDirection.EAST:
                        vector += "WE";
                        number += "E";
                        break;
                }
                SetSwitch(vector, number, true);
            }
            catch
            {
            }
        }

        public void Stop(INDIDirection direction)
        {
            try
            {
                string vector = "TELESCOPE_MOTION_";
                string number = "MOTION_";
                switch (direction)
                {
                    case INDIDirection.NORTH:
                        vector += "NS";
                        number += "N";
                        break;
                    case INDIDirection.SOUTH:
                        vector += "NS";
                        number += "S";
                        break;
                    case INDIDirection.WEST:
                        vector += "WE";
                        number += "W";
                        break;
                    case INDIDirection.EAST:
                        vector += "WE";
                        number += "E";
                        break;
                }
                SetSwitch(vector, number, false);
            }
            catch
            {
            }
        }

        public void PulseGuide(INDIDirection direction, Int64 ms)
        {
            string vector = "TELESCOPE_TIMED_GUIDE_";
            string number = "TIMED_GUIDE_";
            switch (direction)
            {
                case INDIDirection.NORTH:
                    vector += "NS";
                    number += "N";
                    break;
                case INDIDirection.SOUTH:
                    vector += "NS";
                    number += "S";
                    break;
                case INDIDirection.WEST:
                    vector += "WE";
                    number += "W";
                    break;
                case INDIDirection.EAST:
                    vector += "WE";
                    number += "E";
                    break;
            }
            try
            {
                SetNumber(vector, number, ms);
            }
            catch
            {
            }
        }

        public void SyncToAltAz(double Altitude, double Azimuth)
        {
            CoordSet = INDICoordSet.SYNC;
            double[] values = { Altitude, Azimuth };
            SetNumberVector("HORIZONTAL_COORD", values);
		}

		public void SlewToAltAz(double Altitude, double Azimuth)
		{
			CoordSet = INDICoordSet.SLEW;
			double[] values = { Altitude, Azimuth };
			SetNumberVector("HORIZONTAL_COORD", values);
		}

		public void TrackToAltAz(double Altitude, double Azimuth)
		{
			CoordSet = INDICoordSet.TRACK;
			double[] values = { Altitude, Azimuth };
			SetNumberVector("HORIZONTAL_COORD", values);
		}

        public void SyncToRaDec(double RightAscension, double Declination)
        {
            CoordSet = INDICoordSet.SYNC;
            double[] values = { RightAscension, Declination };
            SetNumberVector("EQUATORIAL_EOD_COORD", values);
		}

		public void SlewToRaDec(double RightAscension, double Declination)
		{
			CoordSet = INDICoordSet.SLEW;
			double[] values = { RightAscension, Declination };
			SetNumberVector("EQUATORIAL_EOD_COORD", values);
		}

		public void TrackToRaDec(double RightAscension, double Declination)
		{
			CoordSet = INDICoordSet.TRACK;
			double[] values = { RightAscension, Declination };
			SetNumberVector("EQUATORIAL_EOD_COORD", values);
		}

        public void Abort()
        {
            try
            {
                SetSwitch("TELESCOPE_ABORT_MOTION", "ABORT_MOTION", true);
            }
            catch
            {
            }
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
                    IsNewNumber?.Invoke(this, new INDITelescopeNumberEventArgs(e.Vector, e.Device));
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
                    IsNewSwitch?.Invoke(this, new INDITelescopeSwitchEventArgs(e.Vector, e.Device));
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
                    IsNewText?.Invoke(this, new INDITelescopeTextEventArgs(e.Vector, e.Device));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
        #endregion

        #region Standard Properties
        public double RaJ2000
        {
            get
            {
                try
                {
                    return GetNumber("EQUATORIAL_COORD", "RA").Value;
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                try
                {
                    SetNumber("EQUATORIAL_COORD", "RA", value);
                }
                catch
                {
                }
            }
        }

        public double DecJ2000
        {
            get
            {
                try
                {
                    return GetNumber("EQUATORIAL_COORD", "DEC").Value;
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                try
                {
                    SetNumber("EQUATORIAL_COORD", "DEC", value);
                }
                catch
                {
                }
            }
        }

        public double Ra
        {
            get
            {
                try
                {
                    return GetNumber("EQUATORIAL_EOD_COORD", "RA").Value;
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                try
                {
                    SetNumber("EQUATORIAL_EOD_COORD", "RA", value);
                }
                catch
                {
                }
            }
        }

        public double Dec
        {
            get
            {
                try
                {
                    return GetNumber("EQUATORIAL_EOD_COORD", "DEC").Value;
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                try
                {
                    SetNumber("EQUATORIAL_EOD_COORD", "DEC", value);
                }
                catch
                {
                }
            }
        }

        public double Alt
        {
            get
            {
                try
                {
                    return GetNumber("HORIZONTAL_COORD", "ALT").Value;
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                try
                {
                    SetNumber("HORIZONTAL_COORD", "ALT", value);
                }
                catch
                {
                }
            }
        }

        public double Az
        {
            get
            {
                try
                {
                    return GetNumber("HORIZONTAL_COORD", "AZ").Value;
                }
                catch
                {
                    return 0;
                }
            }
            set
            {
                try
                {
                    SetNumber("HORIZONTAL_COORD", "AZ", value);
                }
                catch
                {
                }
            }
		}

		public bool Parked
		{
			get
			{
				try
				{
					return GetSwitch("TELESCOPE_PARK", "PARK").Value;
				}
				catch
				{
					return false;
				}
			}
			set
			{
				try
				{
					SetSwitchVector("TELESCOPE_PARK", value ? 0 : 1);
				}
				catch
				{
				}
			}
		}

		public bool Track
		{
			get
			{
				try
				{
					return GetSwitch("TELESCOPE_TRACK_STATE", "TRACK_ON").Value;
				}
				catch
				{
					return false;
				}
			}
			set
			{
				try
				{
					SetSwitchVector("TELESCOPE_TRACK_STATE", value ? 0 : 1);
				}
				catch
				{
				}
			}
		}

        public string Diameter
        {
            get
            {
                try
                {
                    return GetText("TELESCOPE_INFO", "TELESCOPE_APERTURE").Value;
                }
                catch
                {
                    throw new ArgumentException();
                }
            }
            set
            {
                try
                {
                    SetText("TELESCOPE_INFO", "TELESCOPE_APERTURE", value);
                }
                catch
                {
                }
            }
        }

        public string FocalLenght
        {
            get
            {
                try
                {
                    return GetText("TELESCOPE_INFO", "TELESCOPE_FOCAL_LENGTH").Value;
                }
                catch
                {
                    throw new ArgumentException();
                }
            }
            set
            {
                try
                {
                    SetText("TELESCOPE_INFO", "TELESCOPE_FOCAL_LENGTH", value);
                }
                catch
                {
                }
            }
        }

        public string GuiderDiameter
        {
            get
            {
                try
                {
                    return GetText("TELESCOPE_INFO", "GUIDER_APERTURE").Value;
                }
                catch
                {
                    throw new ArgumentException();
                }
            }
            set
            {
                try
                {
                    SetText("TELESCOPE_INFO", "GUIDER_APERTURE", value);
                }
                catch
                {
                }
            }
        }

        public string GuiderFocalLenght
        {
            get
            {
                try
                {
                    return GetText("TELESCOPE_INFO", "GUIDER_FOCAL_LENGTH").Value;
                }
                catch
                {
                    throw new ArgumentException();
                }
            }
            set
            {
                try
                {
                    SetText("TELESCOPE_INFO", "GUIDER_FOCAL_LENGTH", value);
                }
                catch
                {
                }
            }
        }

        public INDICoordSet CoordSet
        {
            get
            {
                try
                {
                    return (INDICoordSet)GetSwitchVector("ON_COORD_SET").Index;
                }
                catch
                {
                    return INDICoordSet.TRACK;
                }
            }
            set
            {
                try
                {
                    SetSwitchVector("ON_COORD_SET", (int)value);
                }
                catch
                {
                }
            }
        }

        public INDIRate SlewRate
        {
            get
            {
                try
                {
                    return (INDIRate)GetSwitchVector("TELESCOPE_SLEW_RATE").Index;
                }
                catch
                {
                    return INDIRate.GUIDE;
                }
            }
            set
            {
                try
                {
                    SetSwitchVector("TELESCOPE_SLEW_RATE", (int)value);
                }
                catch
                {
                }
            }
        }

        public INDITrackRate TrackRate
        {
            get
            {
                try
                {
                    return (INDITrackRate)GetSwitchVector("TELESCOPE_TRACK_RATE").Index;
                }
                catch
                {
                    return INDITrackRate.CUSTOM;
                }
            }
            set
            {
                try
                {
                    SetSwitchVector("TELESCOPE_TRACK_RATE", (int)value);
                }
                catch
                {
                }
            }
        }
        #endregion
    }
}