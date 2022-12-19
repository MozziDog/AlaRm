package capstone.Alarm.repository;

import capstone.Alarm.domain.Purchase;
import capstone.Alarm.service.PurchaseService;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.transaction.annotation.Transactional;

import java.util.Date;
import java.util.List;

import static org.junit.jupiter.api.Assertions.*;

@SpringBootTest
class PurchaseRepositoryTest {

    @Autowired
    PurchaseRepository purchaseRepository;

    @Test
    void save() {
        Purchase purchase = new Purchase(15, 1, new Date());
        purchaseRepository.save(purchase);
    }

    @Test
    void find() {
        List<Purchase> byUserId1 = purchaseRepository.findByUserId(15);
        System.out.println(byUserId1);
    }


}