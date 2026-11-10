using System.Reflection;

namespace SmartEMR.ApiPractice.Common;

public class ClassMapper<TSource, TTarget> 
    where TSource : class
    where TTarget : class, new()
{
    public static List<TTarget> MapToList(List<TSource> sources)
    {
        var targetItems = new List<TTarget>();

        foreach (var item in sources)
        {
            targetItems.Add(MapTo(item));
        }

        return targetItems;
    }

    public static TTarget MapTo(TSource source)
    {
        // 변환할 대상(Target) 클래스의 인스턴스 생성
        TTarget target = new TTarget();

        // 소스와 대상의 타입 정보 가져오기
        Type sourceType = source.GetType();
        Type targetType = typeof(TTarget);

        // 대상 클래스의 public 속성(Property)들을 순회
        foreach (PropertyInfo targetProp in targetType.GetProperties(BindingFlags.Public | BindingFlags.Instance))
        {
            // 대상 속성이 값을 쓸 수 있는 상태(set)인지 확인
            if (!targetProp.CanWrite) continue;

            // 소스 클래스에서 이름이 같은 public 속성 찾기
            PropertyInfo? sourceProp = sourceType.GetProperty(targetProp.Name, BindingFlags.Public | BindingFlags.Instance);

            // 소스에 같은 이름의 속성이 있고, 읽을 수 있는 상태(get)인지 확인
            if (sourceProp != null && sourceProp.CanRead)
            {
                // 타입이 호환되는지 확인 (엄격하게 같거나 상속 관계인 경우)
                if (targetProp.PropertyType.IsAssignableFrom(sourceProp.PropertyType))
                {
                    // 값 복사
                    var value = sourceProp.GetValue(source);
                    targetProp.SetValue(target, value);
                }
            }
        }

        return target;
    }
}
