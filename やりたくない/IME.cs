using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Wacton.Desu.Japanese;

namespace やりたくない
{
    static class IME
    {
        public static void init()
        {
            Main.refmain.Window.TextInput += listenForImeChange;
            keytimer.Start();
            japaneseEntries = new JapaneseDictionary().GetEntries().ToList();
        }
        private static imeMode IMEMode = imeMode.none;
        public static string doIMEString(string applyTo, char delta)
        {
            if (IMEMode == imeMode.none)
                return applyTo + delta;
            Dictionary<string, string> valueMutator = IMEMode == imeMode.hiragana ? dualWayMappingHiragana : dualWayMappingKatakana;
            string repeat = IMEMode == imeMode.hiragana ? hiraganaRepeater : katakanaRepeater;

            //get the characters before the added one from the string, empty string if they are \0
            string applyEnd = applyTo.Length > 0 ? applyTo[applyTo.Length - 1].ToString() : "";
            string oneBeforeApplyEnd = applyTo.Length > 1 ? applyTo[applyTo.Length - 2].ToString() : "";

            //check if the triplet/double is in the dictionary
            string possibleCurrentTriplet = oneBeforeApplyEnd + applyEnd + delta.ToString();
            string possibleCurrentDouble = applyEnd + delta.ToString();
            string possibleCurrentSingle = delta.ToString();

            if (possibleCurrentTriplet.Length == 3 && valueMutator.ContainsKey(possibleCurrentTriplet))
            {
                return applyTo.Substring(0, applyTo.Length - 2) + valueMutator[possibleCurrentTriplet];
            }
            else if (possibleCurrentDouble.Length == 2 && valueMutator.ContainsKey(possibleCurrentDouble))
            {
                return applyTo.Substring(0, applyTo.Length - 1) + valueMutator[possibleCurrentDouble];
            }
            else if (possibleCurrentSingle.Length == 1 && valueMutator.ContainsKey(possibleCurrentSingle))
            {
                return applyTo + valueMutator[possibleCurrentSingle];
            }
            else if (punctuationMap.Keys.Contains(delta))
            {
                return applyTo + punctuationMap[delta];
            }
            else if (punctuationNotMappedNoTsu.Contains(delta))
            {
                return applyTo + delta;
            }
            else if (possibleCurrentSingle == applyEnd)
            {
                return applyTo.Substring(0, applyTo.Length - 1) + repeat + possibleCurrentSingle;
            }
            else
            {
                return applyTo + possibleCurrentSingle;
            }
            
        }

        public static string blah(string s)
        {
            return GetPossibleKanji(s).Result.First();
        }

        private static List<IJapaneseEntry> japaneseEntries;

        private static async Task<IEnumerable<String>> GetPossibleKanji(string searchingReading)
        {
            var validEntries = await Task.Run(() => japaneseEntries.Where(entry => entry.Readings.Where(reading => reading.Text == searchingReading).Any()));
            return await Task.Run(() => validEntries.Select(x => x.Kanjis.First().Text));
        }



        enum imeMode
        {
            none,
            hiragana,
            katakana
        }

        private const int keyRepeatTime = 30;//ms
        private static readonly Stopwatch keytimer = new Stopwatch();
        static void listenForImeChange(object sender, TextInputEventArgs e)
        {
            if (keytimer.ElapsedMilliseconds < keyRepeatTime)
                return;
            if (e.Key == Settings.IMEKey)
            {
                switch (IMEMode)
                {
                    case imeMode.none:
                        IMEMode = imeMode.hiragana;
                        break;
                    case imeMode.hiragana:
                        IMEMode = imeMode.katakana;
                        break;
                    case imeMode.katakana:
                        IMEMode = imeMode.none;
                        break;
                }
            }
        }




        private static readonly string katakanaRepeater = "ッ";
        private static readonly Dictionary<string, string> dualWayMappingKatakana = new Dictionary<string, string>()
        {
            {"a","ア"},
            {"i","イ"},
            {"u","ウ"},
            {"e","エ"},
            {"o","オ"},
            {"xa","ァ"},
            {"xi","ィ"},
            {"xu","ゥ"},
            {"xe","ェ"},
            {"xo","ォ"},
            {"xya","ャ"},
            {"xyu","ュ"},
            {"xyo","ョ"},
            {"xwa","ヮ"},
            {"xtsu","ッ"},
            {"xtu","ッ"},
            {"la","ァ"},
            {"li","ィ"},
            {"lu","ゥ"},
            {"le","ェ"},
            {"lo","ォ"},
            {"lya","ャ"},
            {"lyu","ュ"},
            {"lyo","ョ"},
            {"lwa","ヮ"},
            {"ltsu","ッ"},
            {"ltu","ッ"},
            {"ka","カ"},
            {"ki","キ"},
            {"ku","ク"},
            {"ke","ケ"},
            {"ko","コ"},
            {"ga","ガ"},
            {"gi","ギ"},
            {"gu","グ"},
            {"ge","ゲ"},
            {"go","ゴ"},
            {"sa","サ"},
            {"si","シ"},
            {"shi","シ"},
            {"su","ス"},
            {"se","セ"},
            {"so","ソ"},
            {"za","ザ"},
            {"zi","ジ"},
            {"ji","ジ"},
            {"zu","ズ"},
            {"ze","ゼ"},
            {"zo","ゾ"},
            {"ta","タ"},
            {"ti","チ"},
            {"chi","チ"},
            {"tu","ツ"},
            {"tsu","ツ"},
            {"te","テ"},
            {"to","ト"},
            {"da","ダ"},
            {"di","ヂ"},
            {"du","ヅ"},
            {"de","デ"},
            {"do","ド"},
            {"na","ナ"},
            {"ni","ニ"},
            {"nu","ヌ"},
            {"ne","ネ"},
            {"no","ノ"},
            {"ha","ハ"},
            {"hi","ヒ"},
            {"hu","フ"},
            {"fu","フ"},
            {"he","ヘ"},
            {"ho","ホ"},
            {"ba","バ"},
            {"bi","ビ"},
            {"bu","ブ"},
            {"be","ベ"},
            {"bo","ボ"},
            {"pa","パ"},
            {"pi","ピ"},
            {"pu","プ"},
            {"pe","ペ"},
            {"po","ポ"},
            {"ma","マ"},
            {"mi","ミ"},
            {"mu","ム"},
            {"me","メ"},
            {"mo","モ"},
            {"ya","ヤ"},
            {"yu","ユ"},
            {"yo","ヨ"},
            {"ra","ラ"},
            {"ri","リ"},
            {"ru","ル"},
            {"re","レ"},
            {"ro","ロ"},
            {"wa","ワ"},
            {"wo","ヲ"},
            {"nn","ン"},
            {"xn","ン"},
            {"kya","キャ"},
            {"kyu","キュ"},
            {"kyo","キョ"},
            {"gya","ギャ"},
            {"gyu","ギュ"},
            {"gyo","ギョ"},
            {"sya","シャ"},
            {"sha","シャ"},
            {"syu","シュ"},
            {"shu","シュ"},
            {"syo","ショ"},
            {"sho","ショ"},
            {"jya","ジャ"},
            {"ja","ジャ"},
            {"jyu","ジュ"},
            {"ju","ジュ"},
            {"jyo","ジョ"},
            {"jo","ジョ"},
            {"cya","チャ"},
            {"cha","チャ"},
            {"cyu","チュ"},
            {"chu","チュ"},
            {"cyo","チョ"},
            {"cho","チョ"},
            {"dya","ヂャ"},
            {"dyu","ヂュ"},
            {"dyo","ヂョ"},
            {"nya","ニャ"},
            {"nyu","ニュ"},
            {"nyo","ニョ"},
            {"hya","ヒャ"},
            {"hyu","ヒュ"},
            {"hyo","ヒョ"},
            {"bya","ビャ"},
            {"byu","ビュ"},
            {"byo","ビョ"},
            {"pya","ピャ"},
            {"pyu","ピュ"},
            {"pyo","ピョ"},
            {"mya","ミャ"},
            {"myu","ミュ"},
            {"myo","ミョ"},
            {"rya","リャ"},
            {"ryu","リュ"},
            {"ryo","リョ"}
        };

        private static readonly string hiraganaRepeater = "っ";
        private static readonly Dictionary<string, string> dualWayMappingHiragana = new Dictionary<string, string>()
        {
            {"a","あ"},
            {"i","い"},
            {"u","う"},
            {"e","え"},
            {"o","お"},
            {"xa","ぁ"},
            {"xi","ぃ"},
            {"xu","ぅ"},
            {"xe","ぇ"},
            {"xo","ぉ"},
            {"xya","ゃ"},
            {"xyu","ゅ"},
            {"xyo","ょ"},
            {"xwa","ゎ"},
            {"xtsu","っ"},
            {"xtu","っ"},
            {"la","ぁ"},
            {"li","ぃ"},
            {"lu","ぅ"},
            {"le","ぇ"},
            {"lo","ぉ"},
            {"lya","ゃ"},
            {"lyu","ゅ"},
            {"lyo","ょ"},
            {"lwa","ゎ"},
            {"ltsu","っ"},
            {"ltu","っ"},
            {"ka","か"},
            {"ki","き"},
            {"ku","く"},
            {"ke","け"},
            {"ko","こ"},
            {"ga","が"},
            {"gi","ぎ"},
            {"gu","ぐ"},
            {"ge","げ"},
            {"go","ご"},
            {"sa","さ"},
            {"si","し"},
            {"shi","し"},
            {"su","す"},
            {"se","せ"},
            {"so","そ"},
            {"za","さ"},
            {"zi","じ"},
            {"ji","じ"},
            {"zu","ず"},
            {"ze","ぜ"},
            {"zo","ぞ"},
            {"ta","た"},
            {"ti","ち"},
            {"chi","ち"},
            {"tu","つ"},
            {"tsu","つ"},
            {"te","て"},
            {"to","と"},
            {"da","だ"},
            {"di","ぢ"},
            {"du","づ"},
            {"de","で"},
            {"do","ど"},
            {"na","な"},
            {"ni","に"},
            {"nu","ぬ"},
            {"ne","ね"},
            {"no","の"},
            {"ha","は"},
            {"hi","ひ"},
            {"hu","ふ"},
            {"fu","ふ"},
            {"he","へ"},
            {"ho","ほ"},
            {"ba","ば"},
            {"bi","び"},
            {"bu","ぶ"},
            {"be","べ"},
            {"bo","ぼ"},
            {"pa","ぱ"},
            {"pi","ぴ"},
            {"pu","ぷ"},
            {"pe","ぺ"},
            {"po","ぽ"},
            {"ma","ま"},
            {"mi","み"},
            {"mu","む"},
            {"me","め"},
            {"mo","も"},
            {"ya","や"},
            {"yu","ゆ"},
            {"yo","よ"},
            {"ra","ら"},
            {"ri","り"},
            {"ru","る"},
            {"re","れ"},
            {"ro","ろ"},
            {"wa","わ"},
            {"wo","を"},
            {"nn","ん"},
            {"xn","ん"},
            {"kya","きゃ"},
            {"kyu","きゅ"},
            {"kyo","きょ"},
            {"gya","ぎゃ"},
            {"gyu","ぎゅ"},
            {"gyo","ぎょ"},
            {"sya","しゃ"},
            {"sha","しゃ"},
            {"syu","しゅ"},
            {"shu","しゅ"},
            {"syo","しょ"},
            {"sho","しょ"},
            {"jya","じゃ"},
            {"ja","じゃ"},
            {"jyu","じゅ"},
            {"ju","じゅ"},
            {"jyo","じょ"},
            {"jo","じょ"},
            {"cya","ちゃ"},
            {"cha","ちゃ"},
            {"cyu","ちゅ"},
            {"chu","ちゅ"},
            {"cyo","ちょ"},
            {"cho","ちょ"},
            {"dya","ぢゃ"},
            {"dyu","ぢゅ"},
            {"dyo","ぢょ"},
            {"nya","にゃ"},
            {"nyu","にゅ"},
            {"nyo","にょ"},
            {"hya","ひゃ"},
            {"hyu","ひゅ"},
            {"hyo","ひょ"},
            {"bya","びゃ"},
            {"byu","びゅ"},
            {"byo","びょ"},
            {"pya","ぴゃ"},
            {"pyu","ぴゅ"},
            {"pyo","ぴょ"},
            {"mya","みゃ"},
            {"myu","みゅ"},
            {"myo","みょ"},
            {"rya","りゃ"},
            {"ryu","りゅ"},
            {"ryo","りょ"}
        };


        private static readonly HashSet<char> punctuationNotMappedNoTsu = new HashSet<char>()
        {
            '@',
            '#',
            '%',
            '^',
            '&',
            '*',
            '(',
            ')',
            '_',
            '+',
            '=',
            '`',//this is the default IME character, however.
            '{',
            '}',
            '|',
            '\\',
            ';',
            ':',
            '\'',
            '"',
            '<',
            '>',
            '/',
            '$'
        };

        private static readonly Dictionary<char, string> punctuationMap = new Dictionary<char, string>()
        {
            {'~',"～"},
            {'-',"ー"},
            {'!',"！"},
            {'[',"「"},
            {']',"」"},
            {',',"、"},
            {'.',"。"},
            {'?',"?"},
        };
    }
}
