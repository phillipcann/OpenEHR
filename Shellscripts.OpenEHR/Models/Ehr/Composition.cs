using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Shellscripts.OpenEHR.Models.Components;

namespace Shellscripts.OpenEHR.Models.Ehr
{
    public class Composition : Locateable
    {
         
    }

    /*
{

  "archetype_details": {
    "archetype_id": {
      "value": "openEHR-EHR-COMPOSITION.encounter.v1"
    },
    "template_id": {
      "value": "Example.v1::c7ec861c-c413-39ff-9965-a198ebf44747"
    },
    "rm_version": "1.0.2"
  },
  "language": {
    "terminology_id": {
      "value": "ISO_639-1"
    },
    "code_string": "en"
  },
  "territory": {
    "terminology_id": {
      "value": "ISO_3166-1"
    },
    "code_string": "NL"
  },
  "category": {
    "value": "event",
    "defining_code": {
      "terminology_id": {
        "value": "openehr"
      },
      "code_string": "433"
    }
  },
  "composer": {
    "_type": "PARTY_IDENTIFIED",
    "external_ref": {
      "id": {
        "_type": "GENERIC_ID",
        "value": "16b74749-e6aa-4945-b760-b42bdc07098a",
        "scheme": "pid"
      },
      "namespace": "openEHRSys.example.com",
      "type": "PERSON"
    },
    "name": "A name"
  },
  "context": {
    "start_time": {
      "value": "2014-11-18T09:50:35.000+01:00"
    },
    "setting": {
      "value": "other care",
      "defining_code": {
        "terminology_id": {
          "value": "openehr"
        },
        "code_string": "238"
      }
    }
  },
  "content": []
}
     
     */
}
