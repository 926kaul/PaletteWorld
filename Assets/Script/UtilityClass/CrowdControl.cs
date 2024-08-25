using System.Collections;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using Unity.VisualScripting;

public class CC{
    public y_color victim;
    public virtual bool effect(){
        victim.Update_stat();
        return true;
    }
}

public class ncc : CC{
    public ncc(y_color Victim){
        victim = Victim;
    }
}
public class psn : CC{
    public psn(y_color Victim){
        victim = Victim;
    }
    public override bool effect(){
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
    }
    public override bool effect(){
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
    }
    public override bool effect(){
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
    }
    public override bool effect(){
        System.Random rnd = new System.Random();
        int dice = rnd.Next(1,21);
        if(dice < 7) return false;
        return true;
    }
}
public class slp : CC{
    int turn_point;
    public slp(y_color Victim){
        victim = Victim;
        turn_point = 0;
    }
    public override bool effect(){
        turn_point++;
        if(turn_point<3) return false;
        return true;
    }
}
public class frz : CC{
    public frz(y_color Victim){
        victim = Victim;
        victim.C = victim.C/2;
    }
    public override bool effect(){
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
    }
    public override bool effect(){
        turn_point++;
        if(turn_point<=1){
            return false;
        }
        victim.cc = new ncc(victim);
        return true;
    }
}