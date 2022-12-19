package capstone.Alarm.domain;

import capstone.Alarm.form.SignUpForm;
import lombok.Getter;
import lombok.Setter;
import lombok.ToString;

import javax.persistence.*;

@Entity
@Getter
@ToString
public class User {

    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Integer id;

    @Column
    private String loginId;

    @Column
    private String name;

    @Column
    private String password;

    @Column
    private int coin;

    @Column
    private Integer currentState;

    public User() {}

    public User(String loginId, String name, String password, int coin, Integer currentState) {
        this.loginId = loginId;
        this.name = name;
        this.password = password;
        this.coin = coin;
        this.currentState = currentState;
    }


    public User(SignUpForm form) {
        this.loginId = form.getLoginId();
        this.name = form.getName();
        this.password = form.getPassword();
        this.coin = 300;
    }


}
