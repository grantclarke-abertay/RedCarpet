﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using System.ComponentModel;
using static RedCarpet.PropertyGridTypes;
using System.Collections;
using RedCarpet.Gfx;

namespace RedCarpet
{
    public class Object 
    {
        public Dictionary<string, List<MapObject>> mobjs = new Dictionary<string, List<MapObject>>();

        public class MapObject : ICloneable
        {            

            public dynamic this[string v]
            {
                get { return _bymlNode[v];}
                set { _bymlNode[v] = value; }
            }

            [TypeConverter (typeof(DictionaryConverter))]
            public Dictionary<string, dynamic> AllProperties
            {
                get { return _bymlNode; }
                set { _bymlNode = value; }
            }

            [Category("Common properties")]
            public string objectID
            {
                get { return _bymlNode["Id"]; }
                set { _bymlNode["Id"] = value; }
            }

            [Category("Common properties")]
            public string modelName
            {
                get { return _bymlNode["ModelName"]; }
                set { _bymlNode["ModelName"] = value; }
            }

            [Category("Common properties")]
            public string Layer
            {
                get { return _bymlNode["LayerConfigName"]; }
                set { _bymlNode["LayerConfigName"] = value; }
            }

            [Category("Common properties")]
            public string unitConfigName
            {
                get { return _bymlNode["UnitConfigName"]; }
                set { _bymlNode["UnitConfigName"] = value; }
            }

            [Category("Common properties")]
            [TypeConverter(typeof(Vector3Converter))]
            public Vector3 position
            {
                get { return new Vector3(_bymlNode["Translate"]["X"], _bymlNode["Translate"]["Y"], _bymlNode["Translate"]["Z"]); }
                set
                {
                    _bymlNode["Translate"]["X"] = value.X;
                    _bymlNode["Translate"]["Y"] = value.Y;
                    _bymlNode["Translate"]["Z"] = value.Z;
                }
            }

            [Category("Common properties")]
            [TypeConverter(typeof(Vector3Converter))]
            public Vector3 rotation
            {
                get { return new Vector3(_bymlNode["Rotate"]["X"], _bymlNode["Rotate"]["Y"], _bymlNode["Rotate"]["Z"]); }
                set
                {
                    _bymlNode["Rotate"]["X"] = value.X;
                    _bymlNode["Rotate"]["Y"] = value.Y;
                    _bymlNode["Rotate"]["Z"] = value.Z;
                }
            }

            [Category("Common properties")]
            [TypeConverter(typeof(Vector3Converter))]
            public Vector3 scale
            {
                get { return new Vector3(_bymlNode["Scale"]["X"], _bymlNode["Scale"]["Y"], _bymlNode["Scale"]["Z"]); }
                set
                {
                    _bymlNode["Scale"]["X"] = value.X;
                    _bymlNode["Scale"]["Y"] = value.Y;
                    _bymlNode["Scale"]["Z"] = value.Z;
                }
            }

            /*public string Template
            {
                get { return _bymlNode; }
                set { _bymlNode = value; }
            }*/

            public int priority;
            public List<Vector3> vertices = new List<Vector3>();
            public SmBoundingBox boundingBox;

            private Dictionary<string, dynamic> _bymlNode = null;

            public MapObject(dynamic _obj)
            {
                if (!(_obj is Dictionary<string, dynamic>)) throw new Exception("Game object node not supported");
                _bymlNode = _obj;
            }

            public object Clone()
            {
                MapObject o =  new MapObject(CloneDict(_bymlNode));
                o.priority = priority;
                o.vertices = new List<Vector3>();
                foreach (Vector3 v in vertices) o.vertices.Add(v);
                o.boundingBox = boundingBox;
                return o;
            }

            Dictionary<string, dynamic> CloneDict(Dictionary<string, dynamic> dict)
            {
                Dictionary<string, dynamic> res = new Dictionary<string, dynamic>();
                foreach (string k in dict.Keys)
                {
                    if (dict[k] is Dictionary<string, dynamic> && k != "Links") res.Add(k, CloneDict(dict[k])); //Links aren't cloned
                    else if (dict[k] is ICloneable) res.Add(k, ((ICloneable)dict[k]).Clone());
                    else
                    {
                        if (dict[k] != null) System.Diagnostics.Debug.WriteLine("WARNING: CloneDict - " + k + " of type " + ((Type)dict[k].GetType()).ToString() + " is not ICloneable");
                        res.Add(k, dict[k]);
                    }
                }
                return res;
            }
        }
    }
}
