package capstone.Alarm.controller;

import capstone.Alarm.domain.User;
import capstone.Alarm.form.Coin;
import capstone.Alarm.form.LoginForm;
import capstone.Alarm.form.SignUpForm;
import capstone.Alarm.repository.UserRepository;
import capstone.Alarm.security.JwtTokenProvider;
import capstone.Alarm.service.UserService;
import org.springframework.http.HttpHeaders;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.security.NoSuchAlgorithmException;

@RestController
@RequestMapping("/user")
public class UserController {


    private final UserService userService;

    public UserController(UserService userService) {
        this.userService = userService;
    }

    @PostMapping
    public ResponseEntity signUp(@RequestBody SignUpForm form) throws NoSuchAlgorithmException {
        User user = userService.signUp(form);

        if (user == null) {
            return new ResponseEntity(HttpStatus.BAD_REQUEST);
        }

        return new ResponseEntity(HttpStatus.CREATED);
    }

    @PostMapping("/login")
    public ResponseEntity signIn(@RequestBody LoginForm form) throws NoSuchAlgorithmException {
        String token = userService.login(form);

        if (token == null) {
            return new ResponseEntity(HttpStatus.UNAUTHORIZED);
        }

        HttpHeaders headers = new HttpHeaders();
        headers.add("token", token);
        return new ResponseEntity(headers, HttpStatus.CREATED);

    }

    @PostMapping("/change_character/{characterId}")
    public ResponseEntity changeCharacter(@PathVariable Integer characterId, @RequestHeader(value = "token") String token) {

        String loginId = JwtTokenProvider.getUserClaim(token).getLoginId();
        if (userService.updateState(characterId, loginId)) {
            return new ResponseEntity(HttpStatus.OK);
        } else {
            return new ResponseEntity(HttpStatus.FORBIDDEN);
        }
    }

    @GetMapping("/coin")
    public ResponseEntity getCoin(@RequestHeader(value = "token") String token) {
        String loginId = JwtTokenProvider.getUserClaim(token).getLoginId();
        User user = userService.getUser(loginId);
        Coin coin = new Coin(user.getCoin());
        return new ResponseEntity(coin, HttpStatus.OK);
    }

    @PostMapping("/coin")
    public ResponseEntity updateCoin(@RequestHeader(value = "token") String token, @RequestBody Coin coin) {
        String loginId = JwtTokenProvider.getUserClaim(token).getLoginId();
        userService.updateCoin(coin.getCoin(), loginId);
        return new ResponseEntity(HttpStatus.OK);
    }
}
