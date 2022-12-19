package capstone.Alarm.repository;

import capstone.Alarm.domain.Purchase;
import org.springframework.data.jpa.repository.JpaRepository;

import java.util.List;

public interface PurchaseRepository extends JpaRepository<Purchase, PurchaseID> {

    List<Purchase> findByUserId(Integer userId);

}
