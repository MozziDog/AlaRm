package capstone.Alarm.security;

import io.jsonwebtoken.Claims;

public class UserClaim {

    private final String loginId;

    public UserClaim(Claims claims) {
        this.loginId = claims.get("loginId", String.class);
    }

    public String getLoginId() {
        return loginId;
    }

}
