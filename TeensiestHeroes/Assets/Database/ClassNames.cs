using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClassNames : MonoBehaviour
{
    public string[] Names =
    {
        "Duelist",          //SwordSword
        "Thief",            //SwordDagger
        "Adventurer",       //SwordShield
        "Hunter",           //SwordAxe
        "Cavalier",         //SwordSpear
        "Champion",         //SwordBlunt
        "Bounty Hunter",    //SwordCrossbow
        "Slayer",           //SwordThrown
        "Driver",           //SwordWhip
        "Spellsword",       //SwordTome
        "Devout",           //SwordFocus
        "Blink Knight",     //SwordTalisman
        "Enchanted",        //SwordRune
        "Fool",             //SwordWand
        "Night's Watch",    //SwordLantern
        "Mystic",           //SwordOrb
        "Trickster",        //SwordTarot

        "Highwayman",       //DaggerSword
        "Rogue",            //DaggerDagger
        "Soldier",          //DaggerShield
        "Bloodletter",      //DaggerAxe
        "Head Hunter",      //DaggerSpear
        "Primal",           //DaggerBlunt  
        "Stalker",          //DaggerCrossbow
        "Ebonwalker",       //DaggerThrown
        "Hangman",          //DaggerWhip
        "Void Knight",      //DaggerTome
        "Grey Mage",        //DaggerFocus
        "Grey Priest",      //DaggerTalisman
        "Brawler",          //DaggerRune
        "Deceitful",        //DaggerWand
        "Blur",             //DaggerLantern
        "Feint",            //DaggerOrb
        "Illusionist",      //DaggerTarot

        "Knight",           //ShieldSword
        "Pest",             //ShieldDagger
        "Turtle",           //ShieldShield
        "Bramble Knight",   //ShieldAxe
        "Phalanx",          //ShieldSpear
        "Paladin",          //ShieldBlunt  
        "Commander",        //ShieldCrossbow
        "Tactician",        //ShieldThrown
        "Twine Knight",     //ShieldWhip
        "Vector Knight",    //ShieldTome
        "Ward",             //ShieldFocus
        "Blink Knight",     //ShieldTalisman
        "Martial",          //ShieldRune
        "Light Knight",     //ShieldWand
        "Positioneer",      //ShieldLantern
        "Bouncer",          //ShieldOrb
        "Divinator",        //ShieldTarot
        
        "Fighter",          //AxeSword
        "Chopper",          //AxeDagger
        "Raider",           //AxeShield
        "Berserker",        //AxeAxe
        "Render",           //AxeSpear
        "Beater",           //AxeBlunt  
        "Woodsman",         //AxeCrossbow
        "Chucker",          //AxeThrown
        "Rabid",            //AxeWhip
        "Warlock",          //AxeTome
        "Ancestral",        //AxeFocus
        "Porter",           //AxeTalisman
        "Lumberjack",       //AxeRune
        "Strange",          //AxeWand
        "Chaser",           //AxeLantern
        "Pursuer",          //AxeOrb
        "Trapper",          //AxeTarot

        "",          //SpearSword
        "",          //SpearDagger
        "",           //SpearShield
        "",        //SpearAxe
        "",           //SpearSpear
        "",           //SpearBlunt  
        "",         //SpearCrossbow
        "",          //SpearThrown
        "",            //SpearWhip
        "",          //SpearTome
        "",        //SpearFocus
        "",           //SpearTalisman
        "",       //SpearRune
        "",          //SpearWand
        "",           //SpearLantern
        "",          //SpearOrb
        "",          //SpearTarot

        "",          //BluntSword
        "",          //BluntDagger
        "",           //BluntShield
        "",        //BluntAxe
        "",           //BluntSpear
        "",           //BluntBlunt  
        "",         //BluntCrossbow
        "",          //BluntThrown
        "",            //BluntWhip
        "",          //BluntTome
        "",        //BluntFocus
        "",           //BluntTalisman
        "",       //BluntRune
        "",          //BluntWand
        "",           //BluntLantern
        "",          //BluntOrb
        "",          //BluntTarot

        "",          //CrossbowSword
        "",          //CrossbowDagger
        "",           //CrossbowShield
        "",        //CrossbowAxe
        "",           //CrossbowSpear
        "",           //CrossbowBlunt  
        "",         //CrossbowCrossbow
        "",          //CrossbowThrown
        "",            //CrossbowWhip
        "",          //CrossbowTome
        "",        //CrossbowFocus
        "",           //CrossbowTalisman
        "",       //CrossbowRune
        "",          //CrossbowWand
        "",           //CrossbowLantern
        "",          //CrossbowOrb
        "",          //CrossbowTarot

        "",          //ThrownSword
        "",          //ThrownDagger
        "",           //ThrownShield
        "",        //ThrownAxe
        "",           //ThrownSpear
        "",           //ThrownBlunt  
        "",         //ThrownCrossbow
        "",          //ThrownThrown
        "",            //ThrownWhip
        "",          //ThrownTome
        "",        //ThrownFocus
        "",           //ThrownTalisman
        "",       //ThrownRune
        "",          //ThrownWand
        "",           //ThrownLantern
        "",          //ThrownOrb
        "",          //ThrownTarot

        "",          //WhipSword
        "",          //WhipDagger
        "",           //WhipShield
        "",        //WhipAxe
        "",           //WhipSpear
        "",           //WhipBlunt  
        "",         //WhipCrossbow
        "",          //WhipThrown
        "",            //WhipWhip
        "",          //WhipTome
        "",        //WhipFocus
        "",           //WhipTalisman
        "",       //WhipRune
        "",          //WhipWand
        "",           //WhipLantern
        "",          //WhipOrb
        "",          //WhipTarot

        "",          //TomeSword
        "",          //TomeDagger
        "",           //TomeShield
        "",        //TomeAxe
        "",           //TomeSpear
        "",           //TomeBlunt  
        "",         //TomeCrossbow
        "",          //TomeThrown
        "",            //TomeWhip
        "",          //TomeTome
        "",        //TomeFocus
        "",           //TomeTalisman
        "",       //TomeRune
        "",          //TomeWand
        "",           //TomeLantern
        "",          //TomeOrb
        "",          //TomeTarot

        "",          //FocusSword
        "",          //FocusDagger
        "",           //FocusShield
        "",        //FocusAxe
        "",           //FocusSpear
        "",           //FocusBlunt  
        "",         //FocusCrossbow
        "",          //FocusThrown
        "",            //FocusWhip
        "",          //FocusTome
        "",        //FocusFocus
        "",           //FocusTalisman
        "",       //FocusRune
        "",          //FocusWand
        "",           //FocusLantern
        "",          //FocusOrb
        "",          //FocusTarot

        "",          //TalismanSword
        "",          //TalismanDagger
        "",           //TalismanShield
        "",        //TalismanAxe
        "",           //TalismanSpear
        "",           //TalismanBlunt  
        "",         //TalismanCrossbow
        "",          //TalismanThrown
        "",            //TalismanWhip
        "",          //TalismanTome
        "",        //TalismanFocus
        "",           //TalismanTalisman
        "",       //TalismanRune
        "",          //TalismanWand
        "",           //TalismanLantern
        "",          //TalismanOrb
        "",          //TalismanTarot

        "",          //RuneSword
        "",          //RuneDagger
        "",           //RuneShield
        "",        //RuneAxe
        "",           //RuneSpear
        "",           //RuneBlunt  
        "",         //RuneCrossbow
        "",          //RuneThrown
        "",            //RuneWhip
        "",          //RuneTome
        "",        //RuneFocus
        "",           //RuneTalisman
        "",       //RuneRune
        "",          //RuneWand
        "",           //RuneLantern
        "",          //RuneOrb
        "",          //RuneTarot

        "",          //WandSword
        "",          //WandDagger
        "",           //WandShield
        "",        //WandAxe
        "",           //WandSpear
        "",           //WandBlunt  
        "",         //WandCrossbow
        "",          //WandThrown
        "",            //WandWhip
        "",          //WandTome
        "",        //WandFocus
        "",           //WandTalisman
        "",       //WandRune
        "",          //WandWand
        "",           //WandLantern
        "",          //WandOrb
        "",          //WandTarot

        "",          //LanternSword
        "",          //LanternDagger
        "",           //LanternShield
        "",        //LanternAxe
        "",           //LanternSpear
        "",           //LanternBlunt  
        "",         //LanternCrossbow
        "",          //LanternThrown
        "",            //LanternWhip
        "",          //LanternTome
        "",        //LanternFocus
        "",           //LanternTalisman
        "",       //LanternRune
        "",          //LanternWand
        "",           //LanternLantern
        "",          //LanternOrb
        "",          //LanternTarot

        "",          //OrbSword
        "",          //OrbDagger
        "",           //OrbShield
        "",        //OrbAxe
        "",           //OrbSpear
        "",           //OrbBlunt  
        "",         //OrbCrossbow
        "",          //OrbThrown
        "",            //OrbWhip
        "",          //OrbTome
        "",        //OrbFocus
        "",           //OrbTalisman
        "",       //OrbRune
        "",          //OrbWand
        "",           //OrbLantern
        "",          //OrbOrb
        "",          //OrbTarot

        "",          //TarotSword
        "",          //TarotDagger
        "",           //TarotShield
        "",        //TarotAxe
        "",           //TarotSpear
        "",           //TarotBlunt  
        "",         //TarotCrossbow
        "",          //TarotThrown
        "",            //TarotWhip
        "",          //TarotTome
        "",        //TarotFocus
        "",           //TarotTalisman
        "",       //TarotRune
        "",          //TarotWand
        "",           //TarotLantern
        "",          //TarotOrb
        "",          //TarotTarot

        /*
        "",          //NullSword
        "",          //NullDagger
        "",           //NullShield
        "",        //NullAxe
        "",           //NullSpear
        "",           //NullBlunt  
        "",         //NullCrossbow
        "",          //NullThrown
        "",            //NullWhip
        "",          //NullTome
        "",        //NullFocus
        "",           //NullTalisman
        "",       //NullRune
        "",          //NullWand
        "",           //NullLantern
        "",          //NullOrb
        "",          //NullTarot
        */
    };
	
}
