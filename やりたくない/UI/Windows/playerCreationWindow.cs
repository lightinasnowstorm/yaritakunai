using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace やりたくない.UI.Windows
{
    internal static class playerCreationWindow
    {
        public static UIWindow 作る(Vector2 場所, bool tickedUp = false, bool visible = true)
        {
            Textbox nameBox = new Textbox(100, "", Locale.getTRFromKey("PlayerCreate::NamePrompt"));
            Button submitButton = new Button(Locale.getTRFromKey("Game::Create"), delegate
            {
                Main.refmain.setPlayer(new Player(nameBox.text, new PlayerControllers.PlayerInputController(), Vector2.Zero));
                Transitions.SetupAndBegin(Main.refmain.GraphicsDevice, Main.refmain, Transitions.Fade, screens.characterCreate, screens.mainGame);
            });
            Button returnButton = new Button(Locale.getTRFromKey("Game::Back"), delegate
            {
                Transitions.SetupAndBegin(Main.refmain.GraphicsDevice, Main.refmain, Transitions.Fade, screens.characterCreate, screens.characterSelect);
            });
            HorizontalGroupBox hbox = new HorizontalGroupBox(5, 8);
            hbox.Add(submitButton, returnButton);
            UIWindow window = new UIWindow(場所, "player creation", 5, 8, 0)
            {
                new UIText(Locale.getTRFromKey("PlayerCreate::InlineNamePrompt")),
                nameBox,
                hbox
            };


            return window;
        }
    }
}
