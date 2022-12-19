package capstone.Alarm.repository;

import capstone.Alarm.domain.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.data.jpa.repository.Modifying;
import org.springframework.data.jpa.repository.Query;
import org.springframework.data.repository.query.Param;

import java.util.Optional;

public interface UserRepository extends JpaRepository<User, Integer> {
    Optional<User> findByLoginId(String loginId);

    @Modifying(clearAutomatically = true)
    @Query("Update User u set u.coin=:coin where u.loginId=:loginId")
    int updateUserCoin(@Param(value = "coin") Integer coin, @Param(value = "loginId") String loginId);

    @Modifying(clearAutomatically = true)
    @Query("Update User u set u.currentState=:characterId where u.loginId=:loginId")
    int updateCurrentState(@Param(value = "characterId") Integer characterId, @Param(value = "loginId") String loginId);


}
