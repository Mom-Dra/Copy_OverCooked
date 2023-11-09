// 서버-클라이언트 간의 패킷 타입을 분류하기 위해 만든 열거형 타입 
public enum EActionCode
{
    Input, // 단순 키 입력 
    Event, // 플레이어 접속, 씬 로드 등
    Transform, // 오브젝트 이동 제어
    Instantiate, // 오브젝트 생성, 삭제
    Active, // 오브젝트 Active True or False
    Animation, // 오브젝트 애니메이션 제어
    Sound // 사운드 제어 
}