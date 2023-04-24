namespace Lib
{
    public static class Useful
    {
        public static float Remap(float value, float prevMin, float prevMax, float newMin,  float newMax)
        {
            var fromAbs  =  value - prevMin;
            var fromMaxAbs = prevMax - prevMin;      
       
            var normal = fromAbs / fromMaxAbs;
 
            var toMaxAbs = newMax - newMin;
            var toAbs = toMaxAbs * normal;
 
            var to = toAbs + newMin;
       
            return to;
        }
    }
}