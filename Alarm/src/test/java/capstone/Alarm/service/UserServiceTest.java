package capstone.Alarm.service;

import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;

import static org.junit.jupiter.api.Assertions.*;

@SpringBootTest
class UserServiceTest {


    @Autowired
    UserService userService;

//    @Test
//    void updateCoin() {
//        userService.updateCoin(500, "test");
//    }

    @Test
    void updateState() {
        userService.updateState(3, "af");
    }

}