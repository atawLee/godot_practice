using System;

public class RandomUtils
{
    private static readonly Random random = new Random((int)DateTime.Now.Ticks);

    // -PI에서 PI 사이의 임의의 부동 소수점 숫자를 생성합니다.
    public static float RandfRange(float min, float max)
    {
        return (float)(random.NextDouble() * (max - min) + min);
    }
}