package capstone.Alarm.repository;

import capstone.Alarm.domain.User;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.transaction.annotation.Transactional;

import java.util.List;
import java.util.Optional;

@SpringBootTest
@Transactional
class UserRepositoryTest {

    @Autowired
    UserRepository userRepository;

    @Test
    void findAll() {
        Optional<User> byId = userRepository.findById(3);
        System.out.println(byId.get().getLoginId());
    }

    @Test
    void save() {
        User user = new User("a", "oince", "1234", 0, null);
        userRepository.save(user);
        List<User> all = userRepository.findAll();
        for (User user1 : all) {
            System.out.println(user1);
        }
    }

    @Test
    void findByUserId() {
        Optional<User> test = userRepository.findByLoginId("testfdsfgrsgsrtgsrth");
        test.isEmpty();

    }
}