package capstone.Alarm.form;

import lombok.Getter;
import lombok.Setter;

@Getter
@Setter
public class SignUpForm {

    private String loginId;
    private String password;
    private String name;

    public SignUpForm() {}

    public SignUpForm(String loginId, String password, String name) {
        this.loginId = loginId;
        this.password = password;
        this.name = name;
    }


}
