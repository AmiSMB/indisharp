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

namespace INDI
{
    public class Vector
    {
        public string Device { get; set; }
        public string Name { get; set; }
        public string Label { get; set; }
        public string Group { get; set; }
        public string Permission { get; set; }
        public string Rule { get; set; }

        public Vector(string device, string name, string label, string group, string permission, string rule)
        {
            Device = device ?? "";
			Name = name ?? "";
			Label = label ?? "";
			Group = group ?? "";
			Permission = permission ?? "";
			Rule = rule ?? "";
        }
        public void Change(Vector v)
        {
			Device = v.Device ?? Device;
			Name = v.Name ?? Name;
			Label = v.Label ?? Label;
			Group = v.Group ?? Group;
			Permission = v.Permission ?? Permission;
			Rule = v.Rule ?? Rule;
        }
    }
	public class SwitchVector : Vector {
		public List<INDISwitch> Values;
	    public int Index { get; }

	    public SwitchVector(string device, string name, string label, string group, string permission, string rule, List<INDISwitch> v)
            :base(device, name, label, group, permission, rule)
		{
            Values = v;
            if(rule.ToLower() == "oneofmany")
            {
                for (Index = 0; Index < v.Count; Index++)
                    if (v[Index].Value)
                        break;
            }
		}
        public void Change(SwitchVector v)
        {
            base.Change(v);
            Values = v.Values;
        }
	}
	public class TextVector : Vector
    {
        public List<INDIText> Values;
		public TextVector(string device, string name, string label, string group, string permission, string rule, List<INDIText> v)
            : base(device, name, label, group, permission, rule)
        {
            Values = v;
		}
        public void Change(TextVector v)
        {
            base.Change(v);
            Values = v.Values;
        }
	}
	public class NumberVector : Vector
    {
        public List<INDINumber> Values;
		public NumberVector(string device, string name, string label, string group, string permission, string rule, List<INDINumber> v)
            : base(device, name, label, group, permission, rule)
        {
            Values = v;
		}
        public void Change(NumberVector v)
        {
            base.Change(v);
            Values = v.Values;
        }
	}
	public class BlobVector : Vector
    {
        public List<INDIBlob> Values;
        public BlobVector(string device, string name, string label, string group, string permission, string rule, List<INDIBlob> v)
            : base(device, name, label, group, permission, rule)
        {
            Values = v;
		}
        public void Change(BlobVector v)
        {
            base.Change(v);
            Values = v.Values;
        }
	}

	public class INDISwitch {
	    public string Name { get; }
	    public string Label { get; }
	    public bool Value { get; set; }

	    public INDISwitch(string name, string label, bool val)
        {
			Label = label ?? "";
			Name = name ?? "";
            Value = val;
        }
	}

    public class INDIText
    {
        public string Name { get; }
        public string Label { get; }
        public string Value { get; set; }

        public INDIText(string name, string label, string val)
        {
			Label = label ?? "";
			Name = name ?? "";
            Value = val;
        }
	}

    public class INDINumber
    {
        public string Name { get; }
        public string Label { get; }
        public string Format { get; }
        public double Min { get; }
        public double Max { get; }
        public double Step { get; }
        public double Value { get; set; }

        public INDINumber(string name, string label, string format, double minimum, double maximum, double step, double val)
        {
			Label = label ?? "";
			Name = name ?? "";
			Format = format ?? "";
            Value = val;
            Min = minimum;
            Max = maximum;
            Step = step;
        }
	}

    public class INDIBlob
    {
        public string Format { get; }
        public string Name { get; }
        public string Label { get; }
        public byte[] Value { get; set; }
        public int Size { get; }

        public INDIBlob(string name, string label, string format, byte[] val, int length)
        {
			Label = label ?? "";
			Name = name ?? "";
			Format = format ?? "";
            Value = val;
            Size = length;
        }
	}
}

