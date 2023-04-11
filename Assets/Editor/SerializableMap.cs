using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializeableMap
{
    [SerializeField]
    private string valuesAsString;
    private List<string> keys;
    private List<List<string>> values;
    


    public List<string> Keys { get => keys; set => keys = value; }
    public List<List<string>> Values { get => values; set => values = value; }

    public SerializeableMap()
    {
        keys = new List<string>();
        values = new List<List<string>>();
    }

    public List<string> Get(string key)
    {
        int keyIndex = keys.IndexOf(key);
        if (values == null || keyIndex < 0 || keyIndex >= values.Count) { return null; }

        return values[keyIndex];
    }

    public void Add(string key, List<string> value)
    {
        if (!keys.Contains(key))
        {
            keys.Add(key);
        }
        values.Add(value);
    }

    public void Add(string key, string value)
    {
        if(Get(key) == null)
        {
            Add(key, new List<string>() { value });
        }
        else
        {
            if (!values[keys.IndexOf(key)].Contains(value))
            {
                values[keys.IndexOf(key)].Add(value);
            }
        }
    }

    public void RecreateValuesListFromString()
    {
        if(valuesAsString != null && valuesAsString.Length > 0)
        {
            foreach (string keyAndValueString in valuesAsString.Split(","))
            {
                string[] keyAndValue = keyAndValueString.Split(":");
                foreach (string value in keyAndValue[1].Split("##"))
                {
                    if (value == "NONE")
                    {
                        if (keys.IndexOf(keyAndValue[0]) == -1)
                        {
                            Add(keyAndValue[0], new List<string>());
                        }
                        continue;
                    }
                    else
                    {
                        Add(keyAndValue[0], value);
                    }
                }
            }
        }
    }

    //Convert the map into a string representation so that we can serialize the data
    //key:val1##val2##val3##,key1:va1##val3
    public void StoreAsString()
    {
        string result = "";
        for (int i = 0; i < keys.Count; i++)
        {
            string key = keys[i];
            List<string> valueList = values[i];

            result += key + ":";

            if(valueList == null || valueList.Count == 0)
            {
                result += "NONE";
            }
            else
            {
                for (int j = 0; j < valueList.Count; j++)
                {
                    result += valueList[j];
                    if (j != valueList.Count - 1)
                    {
                        result += "##";
                    }
                }
            }

            if (i != keys.Count - 1)
            {
                result += ",";
            }
        }
        Debug.Log(result);
        valuesAsString = result;
    }

    public string AsString()
    {
        return valuesAsString;
    }
}