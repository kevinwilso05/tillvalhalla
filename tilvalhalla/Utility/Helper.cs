namespace TillValhalla
{

    static class helper
    {
        public static float applyModifierValue(float targetValue, float value)
        {

            if (value <= -100)
                value = -100;

            float newValue = targetValue;

            if (value >= 0)
            {
                newValue = targetValue + ((targetValue / 100) * value);
            }
            else
            {
                newValue = targetValue - ((targetValue / 100) * (value * -1));
            }

            return newValue;
        }
    }
}



