package capstone.Alarm.service;

import capstone.Alarm.domain.Purchase;
import capstone.Alarm.domain.User;
import capstone.Alarm.domain.Vcharacter;
import capstone.Alarm.repository.PurchaseID;
import capstone.Alarm.repository.PurchaseRepository;
import capstone.Alarm.repository.UserRepository;
import capstone.Alarm.repository.VcharacterRepository;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

import java.util.ArrayList;
import java.util.Date;
import java.util.List;
import java.util.Optional;

@Service
@Transactional
public class PurchaseService {

    private final PurchaseRepository purchaseRepository;
    private final UserRepository userRepository;
    private final VcharacterRepository vcharacterRepository;

    @Autowired
    public PurchaseService(PurchaseRepository purchaseRepository, UserRepository userRepository, VcharacterRepository vcharacterRepository) {
        this.purchaseRepository = purchaseRepository;
        this.userRepository = userRepository;
        this.vcharacterRepository = vcharacterRepository;
    }

    public Purchase purchaseCharacter(String loginId, int characterId) {
        User user = userRepository.findByLoginId(loginId).get();
        Vcharacter vcharacter = vcharacterRepository.findById(characterId).get();

        if (user.getCoin() < vcharacter.getCost()) {
            return null;
        }

        List<Purchase> byUserId = purchaseRepository.findByUserId(user.getId());

        for (Purchase purchase : byUserId) {
            if (purchase.getCharacterId() == characterId) {
                return null;
            }
        }

        userRepository.updateUserCoin(user.getCoin() - vcharacter.getCost(), user.getLoginId());

        Purchase purchase = new Purchase(user.getId(), characterId, new Date());
        purchaseRepository.save(purchase);
        return purchase;
    }

    public List<Integer> findOwnList(String loginId) {
        Integer id = userRepository.findByLoginId(loginId).get().getId();
        List<Integer> list = new ArrayList<>();

        List<Purchase> byUserId = purchaseRepository.findByUserId(id);

        for (Purchase purchase : byUserId) {
            list.add(purchase.getCharacterId());
        }

        return list;
    }

    public Optional<Purchase> findById(PurchaseID id) {
        return purchaseRepository.findById(id);
    }

}
