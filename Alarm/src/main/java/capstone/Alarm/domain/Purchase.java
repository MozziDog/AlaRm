package capstone.Alarm.domain;

import capstone.Alarm.repository.PurchaseID;
import lombok.Getter;
import lombok.ToString;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Id;
import javax.persistence.IdClass;
import java.sql.Timestamp;
import java.util.Date;

@Entity
@Getter
@ToString
@IdClass(PurchaseID.class)
public class Purchase {

    @Id
    private Integer userId;

    @Id
    private Integer characterId;

    @Column
    private Date purchaseDate;

    public Purchase() {}

    public Purchase(Integer userId, Integer characterId, Date purchaseDate) {
        this.userId = userId;
        this.characterId = characterId;
        this.purchaseDate = purchaseDate;
    }
}
