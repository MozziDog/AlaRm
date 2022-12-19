package capstone.Alarm.controller;

import capstone.Alarm.domain.Purchase;
import capstone.Alarm.security.JwtTokenProvider;
import capstone.Alarm.service.PurchaseService;
import com.fasterxml.jackson.core.JsonProcessingException;
import org.json.simple.JSONObject;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.util.List;

@RestController
@RequestMapping("/character")
public class CharacterController {

    private final PurchaseService purchaseService;

    public CharacterController(PurchaseService purchaseService) {
        this.purchaseService = purchaseService;
    }

    @PostMapping("/{characterId}")
    public ResponseEntity purchase(@PathVariable Integer characterId, @RequestHeader(value = "token") String token) {
        String loginId = JwtTokenProvider.getUserClaim(token).getLoginId();

        if (characterId <= 0 || 3 <= characterId) {
            return new ResponseEntity(HttpStatus.BAD_REQUEST);
        }

        Purchase purchase = purchaseService.purchaseCharacter(loginId, characterId);

        if (purchase == null) {
            return new ResponseEntity(HttpStatus.BAD_REQUEST);
        }

        return new ResponseEntity(HttpStatus.CREATED);

    }


    @GetMapping("/own")
    public ResponseEntity ownList(@RequestHeader(value = "token") String token) throws JsonProcessingException {
        String loginId = JwtTokenProvider.getUserClaim(token).getLoginId();

        List<Integer> ownList = purchaseService.findOwnList(loginId);

        JSONObject obj = new JSONObject();
        obj.put("characters", ownList);

        return new ResponseEntity(obj, HttpStatus.OK);
    }

}
