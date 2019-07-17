import sys
import json

result = dict()

with open(sys.argv[1]) as thesaurus:
    print(f"Thesaurus: {sys.argv[1]}")
    with open(sys.argv[2]) as dictionary:
        print(f"Dictionary: {sys.argv[2]}")
        common_words = set([w.strip() for w in dictionary.readlines() if w[0].islower()])
        print(f"{len(common_words)} common words");
        for line in thesaurus:

            if not line[0].islower():
                continue

            words = line.split(",")
            first = words[0]
            section = result.get(first[0], dict())

            if first in common_words:
                print(f"Saving: {first}")
                related = [word for word in words[1:] if word in common_words]
                section[first] = related

            result[first[0]] = section

for section, data in result.items():
    with open(f"./json/thesaurus-{section}.json", 'w') as output:
        output.write(json.dumps(data))
