using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Unity.VisualScripting;

public class CC{
    public y_color victim;
    public virtual bool effect(int hit_dice){
        return true;
    }
    public Color cc_color;
}

public class ncc : CC
{
    public ncc(y_color Victim)
    {
        victim = Victim;
        this.cc_color = new Color32(0, 0, 0, 255);
    }
    public override bool effect(int hit_dice){
        victim.recover_stat();
        return true;
    }
}
public class psn : CC{
    public psn(y_color Victim){
        victim = Victim;
        this.cc_color = new Color32(128,255,0,255);
        if(victim.type1 == 7 || victim.type2 == 7){
            victim.cc = new ncc(victim);
        }
    }
    public override bool effect(int hit_dice){
        victim.hp -= (victim.H+55)/8;
        if(victim.hp<=0){
            Object.Destroy(victim.gameObject);
        }
        return true;
    }
}
public class ppsn : CC{
    int turn_point;
    public ppsn(y_color Victim){
        victim = Victim;
        turn_point = 1;
        this.cc_color = new Color32(128,0,128,255);
        if(victim.type1 == 7 || victim.type2 == 7){
            victim.cc = new ncc(victim);
        }
    }
    public override bool effect(int hit_dice){
        victim.hp -= (victim.H+55)*turn_point/16;
        turn_point++;
        if(victim.hp<=0){
            Object.Destroy(victim.gameObject);
        }
        return true;
    }
}
public class brn : CC{
    public brn(y_color Victim){
        victim = Victim;
        victim.A = victim.A/2;
        this.cc_color = new Color32(255,0,0,255);
        if(victim.type1 == 1 || victim.type2 == 1 || victim.type1 == 18 || victim.type2 == 18){
            victim.cc = new ncc(victim);
        }
    }
    public override bool effect(int hit_dice){
        victim.hp -= (victim.H+55)/16;
        if(victim.hp<=0){
            Object.Destroy(victim.gameObject);
        }
        return true;
    }
}
public class par : CC{
    public par(y_color Victim){
        victim = Victim;
        victim.S = victim.S/2;
        this.cc_color = new Color32(255,255,0,255);
        if(victim.type1 == 4 || victim.type2 == 4 || victim.type1 == 21 || victim.type2 == 21){
            victim.cc = new ncc(victim);
        }
    }
    public override bool effect(int hit_dice){
        if(hit_dice < 8) return false;
        return true;
    }
}
public class slp : CC{
    int turn_point;
    public slp(y_color Victim){
        victim = Victim;
        turn_point = 0;
        this.cc_color = new Color32(255,0,255,255);
        
    }
    public override bool effect(int hit_dice){
        turn_point++;
        if(turn_point >= 3 || hit_dice == 20){
            victim.cc = new ncc(victim);
            return true;
        }
        return false;
    }
}
public class frz : CC{
    public frz(y_color Victim){
        victim = Victim;
        victim.C = victim.C/2;
        this.cc_color = new Color32(0,255,255,255);
        if(victim.type1 == 5 || victim.type2 == 5 || victim.type1 == 22 || victim.type2 == 22){
            victim.cc = new ncc(victim);
        }
    }
    public override bool effect(int hit_dice){
        victim.hp -= (victim.H+55)/16;
        if(victim.hp<=0){
            Object.Destroy(victim.gameObject);
        }
        return true;
    }
}
public class rbd: CC{
    int turn_point;
    public rbd(y_color Victim){
        victim = Victim;
        turn_point = 0;
        this.cc_color = new Color32(0,0,128,255);
    }
    public override bool effect(int hit_dice){
        turn_point++;
        if(turn_point>1 || hit_dice == 20){
            victim.cc = new ncc(victim);
            return true;
        }
        if(turn_point==1){
            victim.cc = new ncc(victim);
            return false;
        }
        return false;
    }
}