///Useful tips there: https://youtu.be/FwOxLkJTXag

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class CustomRuleTileIT : RuleTile<RuleTile.TilingRuleOutput.Neighbor> {
    public bool customField;

//    public class Neighbor : RuleTile.TilingRule.Neighbor {
        //public const int Null = 3;
        //public const int NotNull = 4;
    //}

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case TilingRuleOutput.Neighbor.This: return tile != null;
			case TilingRuleOutput.Neighbor.NotThis: return tile == null;
        }
        return base.RuleMatch(neighbor, tile);
    }
}