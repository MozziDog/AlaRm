package capstone.Alarm.service;

import capstone.Alarm.domain.Purchase;
import capstone.Alarm.domain.User;
import capstone.Alarm.form.LoginForm;
import capstone.Alarm.form.SignUpForm;
import capstone.Alarm.repository.PurchaseID;
import capstone.Alarm.repository.PurchaseRepository;
import capstone.Alarm.repository.UserRepository;
import capstone.Alarm.security.JwtTokenProvider;
import capstone.Alarm.security.SHA256;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.security.NoSuchAlgorithmException;
import java.util.Optional;

@Service
@Transactional
public class UserService {

    private final UserRepository userRepository;
    private final PurchaseService purchaseService;

    public UserService(UserRepository userRepository, PurchaseService purchaseService) {
        this.userRepository = userRepository;
        this.purchaseService = purchaseService;
    }

    public User signUp(SignUpForm form) throws NoSuchAlgorithmException {

        if(isExist(form.getLoginId())) {
            return null;
        }

        String encrypt = SHA256.encrypt(form.getPassword());
        form.setPassword(encrypt);

        User user = new User(form);
        userRepository.save(user);
        purchaseService.purchaseCharacter(user.getLoginId(), 1);
        return user;
    }

    private boolean isExist(String userId) {
        return userRepository.findByLoginId(userId).isPresent();
    }

    public String login(LoginForm form) throws NoSuchAlgorithmException {

        Optional<User> user = userRepository.findByLoginId(form.getLoginId());

        if (user.isEmpty()) {
            return null;
        }

        String encrypt = SHA256.encrypt(form.getPassword());
        form.setPassword(encrypt);

        if (user.get().getPassword().equals(form.getPassword())) {
            return JwtTokenProvider.createToken(form.getLoginId());
        } else {
            return null;
        }
    }

    public User getUser(String loginId) {
        return userRepository.findByLoginId(loginId).get();
    }



    public void updateCoin(int changedCoin, String loginId) {
        userRepository.updateUserCoin(changedCoin, loginId);
    }

    public boolean updateState(int characterId, String loginId) {
        User user = getUser(loginId);
        if (purchaseService.findById(new PurchaseID(user.getId(), characterId)).isPresent()) {
            userRepository.updateCurrentState(characterId, loginId);
            return true;
        } else {
            return false;
        }
    }

}
