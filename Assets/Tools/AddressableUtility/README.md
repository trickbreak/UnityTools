# AddressableUtility

## AddressablePools

### 사전 조건
- 풀링 처리를 하고 싶은 GameObject에 PoolingTarget 컴포넌트가 붙어 있어야 합니다. 
- 풀링 처리를 하고 싶은 GameObject에 어드레서블Key가 등록되어 있어야 합니다.

### 사용시 샘플 코드
```csharp
// 오브젝트 가져오기
PoolingTarget target = AddressablePools.Get("어드레서블Key");

// 오브젝트 돌려주기
target.Release();
```

### PoolingTarget
- AddressablePools에서 오브젝트를 가져올때 / 돌려줄때 이벤트를 등록할 수 있습니다.
  - OnGetUnityEvent 
  - OnReleaseUnityEvent

### PoolingTarget 이벤트 샘플

```csharp
public class Bullet : MonoBehaviour
{
    public void GetEvent()
    {
        Debug.Log("Get!");
    }
    
    public void ReleaseEvent()
    {
        Debug.Log("Release!");
    }
}
```

![sample_001.png](./Documentation~/sample_001.png)
