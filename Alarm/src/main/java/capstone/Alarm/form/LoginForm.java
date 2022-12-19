package capstone.Alarm.form;

import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class LoginForm {

    private String loginId;
    private String password;

    public LoginForm() {
    }

    public LoginForm(String loginId, String password) {
        this.loginId = loginId;
        this.password = password;
    }
}
