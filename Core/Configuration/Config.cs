﻿using Lomztein.Moduthulhu.Core.Bot;
using Lomztein.Moduthulhu.Core.IO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;

namespace Lomztein.Moduthulhu.Core.Configuration {

    /// <summary>
    /// This class might be overengineered, and is subject to change.
    /// </summary>
    public abstract class Config {

        public static string configRootDirectory = AppContext.BaseDirectory + "/Configuration/";

        protected Dictionary<ulong, Dictionary<string, object>> entries = new Dictionary<ulong, Dictionary<string, object>> ();

        public string name;

        public Config(string _name) {
            name = _name;
        }

        public Config() {
            name = "Unnamed Config";
        }

        public abstract void Load();

        public abstract void Save();

        public T GetEntry<T>(ulong id, string key, T fallback) {
            Log.Write (Log.Type.CONFIG, "Getting config: " + id + "-" + key);
            SetEntryIfEmpty (id, key, fallback);
            return JSONSerialization.ConvertObject<T> (entries[id][key]);
        }

        public void SetEntry(ulong id, string key, object obj, bool save) {

            SetEntryIfEmpty (id, key, obj);
            entries [ id ] [ key ] = obj;

            if (save)
                Save ();
        }

        private void SetEntryIfEmpty (ulong id, string key, object fallback) {
            if (!entries.ContainsKey (id)) {
                entries.Add (id, new Dictionary<string, object> ());
            }

            if (!entries [ id ].ContainsKey (key)) {
                entries [ id ].Add (key, fallback);
            }
        }

        public string GetPath() {
            return configRootDirectory + name;
        }
    }
}
