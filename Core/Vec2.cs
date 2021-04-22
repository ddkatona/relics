using UnityEngine;

public class Vec2 {
    public int x;
    public int y;

    public int Legnth => Mathf.Abs(x) + Mathf.Abs(y);

    public Vec2(int x, int y) {
        this.x = x;
        this.y = y;
    }

    public Vec2 Plus(Vec2 other) {
        x += other.x;
        y += other.y;
        return this;
    }

    public Vec2 Minus(Vec2 other) {
        x -= other.x;
        y -= other.y;
        return this;
    }

    public bool DivisibleBy(int n) {
        return x % n == 0 && y % n == 0;
    }

    public void DivideBy(int n) {
        x /= n;
        y /= n;
    }

    public Vec2 Invert() {
        x *= -1;
        y *= -1;
        return this;
    }

}