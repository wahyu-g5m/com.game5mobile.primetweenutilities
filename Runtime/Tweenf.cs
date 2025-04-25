using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using PrimeTween;

public static class Tweenf
{
    public static Sequence JellyPunchScale(Transform target, float strength, float duration, System.Action onPunched = null)
    {
        strength = Mathf.Clamp(strength, 0f, 1f);
        var startValue = target.localScale;
        var strengthenedScale = startValue * (1 + strength);
        var weakenedScale = startValue * (1 - strength);
        return Sequence.Create()
            .Insert(0f, Tween.ScaleX(target, weakenedScale.x, duration * 0.25f))
            .Insert(0f, Tween.ScaleY(target, strengthenedScale.y, duration * 0.25f))
            .Insert(duration * 0.25f, Tween.ScaleX(target, strengthenedScale.x, duration * 0.25f))
            .Insert(duration * 0.25f, Tween.ScaleY(target, weakenedScale.y, duration * 0.25f))
            .Insert(duration * 0.5f, Tween.ScaleX(target, weakenedScale.x, duration * 0.25f))
            .Insert(duration * 0.5f, Tween.ScaleY(target, strengthenedScale.y, duration * 0.25f))
            .ChainCallback(() => onPunched?.Invoke())
            .Insert(duration * 0.75f, Tween.ScaleX(target, startValue.x, duration * 0.25f, Easing.Bounce(5f)))
            .Insert(duration * 0.75f, Tween.ScaleY(target, startValue.y, duration * 0.25f, Easing.Bounce(5f)));
    }

    public static Sequence JellyPunchScaleVariant(Transform target)
    {
        var sequence = Sequence.Create();
        sequence.Chain(Tween.ScaleY(target, 1.2f, 0.35f, Ease.InOutSine)).Group(Tween.ScaleX(target, 0.9f, 0.35f, Ease.InOutSine));
        sequence.Chain(Tween.ScaleY(target, 1f, 1.05f,Easing.Elastic(1.25f, 0.425f))).Group(Tween.ScaleX(target, 1f, 1.25f, Easing.Elastic(1.25f, 0.425f)));
        sequence.timeScale = 2.35f;
        return sequence;
    }


    public static Tween CubicBezier([NotNull] Transform target, Vector3 startValue, Vector3 controlValue1, Vector3 controlValue2, Vector3 endValue, float duration, AnimationCurve animationCurve)
        => Tween.Custom(0f, 1f, duration, (x) =>
        {
            x = animationCurve.Evaluate(x);
            var targetPos = CubicBezier(startValue, controlValue1, controlValue2, endValue, x);
            target.position = targetPos;
        });

    public static Vector3 CubicBezier(Vector3 start, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
         float u = 1 - t;
         float tt = t * t;
         float uu = u * u;
         float uuu = uu * u;
         float ttt = tt * t;

         Vector3 p = uuu * start; // u^3 * start
         p += 3 * uu * t * p1; // 3 * u^2 * t * control1
         p += 3 * u * tt * p2; // 3 * u * t^2 * control2
         p += ttt * p3; // t^3 * end

         return p;
    }

}