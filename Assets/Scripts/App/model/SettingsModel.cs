using UnityEngine;
using System.Collections;

namespace Assets.Scripts.App
{
// Clase que contiene las viarables de Settings
    public class SettingsModel
    {
        // Indica el modo actual. Si el valor es 0, el modo es Libre. Si el valor es 1, el modo es Desafio.
        public int currentMode;
        // Indica el nombre del usuario al que luego se le guardaran sus metricas
        public string userName;
        // Indica el lenguaje del juego. 0 es Espanio y 1 es Ingles
        public int language;
        // Indica si los sonidos estan activados o no
        public bool soundEffects;
        // Indica si la musica esta activada o no
        public bool music;

		public SettingsModel()
        {
            currentMode = 0;
            userName = "";
			language = 0; // Por default es espaniol
			//language = systemLanguage == SystemLanguage.Spanish ? 0 : 1;
            soundEffects = true;
            music = true;
        }
    
    }
}
