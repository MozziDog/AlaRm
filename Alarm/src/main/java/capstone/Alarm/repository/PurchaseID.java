package capstone.Alarm.repository;

import lombok.Data;
import lombok.Getter;
import lombok.Setter;

import java.io.Serializable;

@Data
public class PurchaseID implements Serializable {
    private Integer userId;
    private Integer characterId;

    public PurchaseID() {}

    public PurchaseID(Integer userId, Integer characterId) {
        this.userId = userId;
        this.characterId = characterId;
    }
}
