using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using System.IO;
using Newtonsoft.Json;

namespace やりたくない
{
    [Serializable]
    internal class Item
    {
        public const int numItems = 2;
        public static Texture2D[] itemTextures = new Texture2D[numItems];

        public void use() => itemPrototypes[id].doAction();

        public ushort id;

        public Item(ushort Id)
        {
            id = Id;
        }


        public static void init()
        {
            resetActions();
            resetLanguage();
            resetActionDelegates();
            Locale.onLocaleLoadOrChange += resetLanguage;
        }

        public static void resetActions()
        {
            //load the item prototypes from the json definition file
            string allJson = new StreamReader(Assembly.GetEntryAssembly().GetManifestResourceStream("やりたくない.Resources.itemActions.json")).ReadToEnd();
            dynamic itemActions = JsonConvert.DeserializeObject(allJson);
            foreach (var keyValuePair in itemActions)
            {
                ushort itemActionAddedTo = UInt16.Parse(keyValuePair.Name);
                string itemAction = keyValuePair.Value.Value;
                itemPrototypes[itemActionAddedTo].action = itemAction;
            }
        }

        public static void resetLanguage()
        {
            for (int i = 0; i < numItems; i++)
            {
                itemPrototypes[i].name = Locale.getTRFromKey($"Items::Names[{i}]");
                itemPrototypes[i].description = Locale.getTRFromKey($"Items::Descriptions[{i}]");
            }
        }
        private static void resetActionDelegates()
        {
            actions.Clear();
            actions.Add("blizzard", delegate
            {
                new Projectile(projectileType.blizzardPiece, Input.mouseWorldspace, NoController.noController, 25, DamageType.True);
                new Projectile(projectileType.blizzardPiece, Input.mouseWorldspace - new Vector2(0, 200), NoController.noController, 25, DamageType.True, 1, (float)Math.PI / 3);
            });
        }

        public static Dictionary<string, yaritakunaiEventHandler> actions = new Dictionary<string, yaritakunaiEventHandler>();
        static itemPrototype[] itemPrototypes = new itemPrototype[numItems];
    }

    internal struct itemPrototype
    {
        /// <summary>
        /// this is the general constructor for a prototype item
        /// </summary>
        /// <param name="number"></param>
        /// <param name="Action"></param>
        public itemPrototype(ushort number, string Action = null)
        {
            name = Locale.getTRFromKey($"Items::Names[{number}]");
            description = Locale.getTRFromKey($"Items::Descriptions[{number}]");
            action = Action;
        }
        public string name;
        public string description;
        public string action;
        public void doAction()
        {
            //this depends on the short-circuiting operator, the null check must be on the left
            if (action != null && Item.actions.ContainsKey(action))
            {
                Item.actions[action]();
            }
        }
    }
}
